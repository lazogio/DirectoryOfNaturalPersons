using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonById;

public sealed record GetPersonByIdQuery(
    int PersonId
) : IRequest<PersonDetailedModel>;