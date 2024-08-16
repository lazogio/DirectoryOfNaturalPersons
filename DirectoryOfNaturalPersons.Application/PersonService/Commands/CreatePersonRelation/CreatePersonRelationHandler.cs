using AutoMapper;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePersonRelation;

public class CreatePersonRelationHandler : IRequestHandler<CreatePersonRelationCommand, RelatedPersonsModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;
    private readonly IMapper _mapper;


    public CreatePersonRelationHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
        _mapper = mapper;
    }

    public async Task<RelatedPersonsModel> Handle(CreatePersonRelationCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _repository.GerPersonDetailByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(message + $"{request.PersonId}", true);
        }

        var relatedPerson = await _repository.GerPersonDetailByIdAsync(request.RelatedPersonId, cancellationToken);
        if (relatedPerson is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
            throw new NotFoundException(message + $"{request.RelatedPersonId}", true);
        }

        _repository.UpdatePerson(relatedPerson);

        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<RelatedPersonsModel>(relatedPerson);
    }
}