using System.Net;
using AutoMapper;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePerson;

public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonModel>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IResourceManagerService _resourceManagerService;

    public CreatePersonHandler(IPersonRepository personRepository, IMapper mapper, IUnitOfWork unitOfWork,
        IResourceManagerService resourceManagerService)
    {
        _repository = personRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var existingPerson = await _repository.GetPersonByPersonalIdAsync(request.PersonalId, cancellationToken);
        if (existingPerson is not null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonWithPersonalIdAlreadyExists);
            throw new HttpException(message + $"{request.PersonalId}", HttpStatusCode.Conflict);
        }

        var entityPerson = _mapper.Map<PersonDTO>(request);

        await _repository.InsertAsync(entityPerson, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var city = await _repository.GerCityIdAsync(request.CityId, cancellationToken);

        var person = _mapper.Map<PersonModel>(entityPerson);
        person.City = city!.NameEn;

        return person;
    }
}