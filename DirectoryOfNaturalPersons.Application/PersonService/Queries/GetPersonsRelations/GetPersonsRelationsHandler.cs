using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
using MediatR;

namespace DirectoryOfNaturalPersons.Application.PersonService.Queries.GetPersonsRelations;

public class
    GetPersonsRelationsHandler : IRequestHandler<GetPersonsRelationsQuery, IQueryable<PersonRelationReportModel>>
{
    private readonly IPersonRepository _repository;

    public GetPersonsRelationsHandler(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IQueryable<PersonRelationReportModel>> Handle(GetPersonsRelationsQuery request,
        CancellationToken cancellationToken)
    {
        
        return await _repository.GetPersonsRelationsAsync(cancellationToken);
    }
}