using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.DeletePersonRelation;

public sealed record DeletePersonRelationCommand(
    int PersonId,
    int RelatedPersonId
) : IRequest<ResponseModel>;