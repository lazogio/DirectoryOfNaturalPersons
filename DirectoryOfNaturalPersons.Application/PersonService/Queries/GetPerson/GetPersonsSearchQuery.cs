using DirectoryOfNaturalPersons.Domain.GenericModel;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPerson;

public sealed record GetPersonsSearchQuery(
    string? QuickSearch,
    string? FirstName,
    string? LastName,
    string? PersonalId,
    int? Page,
    int? PageSize) : IRequest<PagedResult<PersonModel>>;