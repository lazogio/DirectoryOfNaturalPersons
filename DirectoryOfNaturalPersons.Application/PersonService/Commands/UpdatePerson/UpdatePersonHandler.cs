using AutoMapper;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.UpdatePerson;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;
    private readonly IMapper _mapper;

    public UpdatePersonHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
        _mapper = mapper;
    }

    public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GerPersonDetailByIdAsync(request.Id, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(message + $"{request.Id}", true);
        }

        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.PersonalId = request.PersonalId;
        person.Gender = request.Gender;
        person.CityId = request.CityId;
        person.BirthDate = request.BirthDate;
        person.UpdateDate = DateTime.Now;

        foreach (var phoneNumberModel in request.PhoneNumbers.OfType<UpdatePhoneNumberModel>())
        {
            var phoneNumber = phoneNumberModel;
            var dbPhoneNumber = person.PhoneNumbers.FirstOrDefault(x => x.Id == phoneNumber.Id);

            if (dbPhoneNumber is not null)
            {
                dbPhoneNumber.Number = phoneNumber.Number;
                dbPhoneNumber.NumberType = phoneNumber.NumberType;
                dbPhoneNumber.UpdateDate = DateTime.Now;
            }
        }

        _repository.UpdatePerson(person);
        await _unitOfWork.CommitAsync(cancellationToken);

        var city = await _repository.GerCityIdAsync(request.CityId, cancellationToken);
        var model = _mapper.Map<PersonModel>(person);
        model.City = city!.NameEn;

        return model;
    }
}