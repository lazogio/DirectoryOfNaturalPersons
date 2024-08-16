using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonsRelations;

public sealed record GetPersonsRelationsQuery : IRequest<IQueryable<PersonRelationReportModel>>;