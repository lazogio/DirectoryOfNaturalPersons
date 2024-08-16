using System.Net;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePersonRelation;

public class DeletePersonRelationHandler : IRequestHandler<DeletePersonRelationCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public DeletePersonRelationHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<ResponseModel> Handle(DeletePersonRelationCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GerPersonDetailByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(message + $"{request.PersonId}", true);
        }

        var relatedPerson = person.RelatedPersons.FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId);
        if (relatedPerson is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
            throw new NotFoundException(message + $"{request.RelatedPersonId}", true);
        }

        person.RelatedPersons.Remove(relatedPerson);
        _repository.DeletePersonRelation(relatedPerson);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseModel("Person Relation deleted successfully.", HttpStatusCode.OK);
    }
}