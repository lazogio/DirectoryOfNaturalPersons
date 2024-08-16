using System.Net;
using DirectoryOfNaturalPersons.Application.Interface;
using DirectoryOfNaturalPersons.Domain.Constants;
using DirectoryOfNaturalPersons.Domain.Exceptions;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePerson;

public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public DeletePersonHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<ResponseModel> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GerPersonByIdAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new NotFoundException(message + $"{request.PersonId}", true);
        }

        _repository.DeletePerson(person);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new ResponseModel("Person : " + $"{person.PersonalId}" + " deleted successfully.", HttpStatusCode.OK);
    }
}