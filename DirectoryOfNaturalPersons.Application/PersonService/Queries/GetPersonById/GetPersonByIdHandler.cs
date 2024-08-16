using AutoMapper;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonById;

public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDetailedModel>
{
    private readonly IMapper _mapper;
    private readonly IPersonRepository _personRepository;
    private readonly IResourceManagerService _resourceManagerService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetPersonByIdHandler(IMapper mapper, IPersonRepository personRepository,
        IResourceManagerService resourceManagerService, IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment)
    {
        _mapper = mapper;
        _personRepository = personRepository;
        _resourceManagerService = resourceManagerService;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<PersonDetailedModel> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var entityPerson = await _personRepository.GerPersonDetailByIdAsync(request.PersonId, cancellationToken);
        if (entityPerson is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(message + $"{request.PersonId}", true);
        }

        var person = _mapper.Map<PersonDetailedModel>(entityPerson);
        var city = await _personRepository.GerCityIdAsync(entityPerson.CityId, cancellationToken);
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        person.City = headers!["Accept-Language"].Contains("ka-GE") ? city!.NameKa : city!.NameEn;

        string fileName = entityPerson.FirstName + "_" + entityPerson.LastName + "_" + entityPerson.Id;
        var wwwRootPath = _webHostEnvironment.WebRootPath;
        var searchPath = Path.Combine(wwwRootPath, "images");
        var filePath = Directory.GetFiles(searchPath, fileName + ".jpg", SearchOption.AllDirectories).FirstOrDefault();

        person.FilePath = filePath;

        return person;
    }
}