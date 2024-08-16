using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePerson;

public sealed record DeletePersonCommand(
    int PersonId
) : IRequest<ResponseModel>;