using DirectoryOfNaturalPersons.Domain.Enums;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Commands.CreatePersonRelation;

public sealed record CreatePersonRelationCommand(
    int PersonId,
    int RelatedPersonId,
    RelationType RelationType
) : IRequest<RelatedPersonsModel>;