using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.GenericModel;
using DirectoryOfNaturalPersons.Domain.Models;

namespace DirectoryOfNaturalPersons.Domain.Interface;

public interface IPersonRepository
{
    Task InsertAsync(PersonDTO entity, CancellationToken cancellationToken);
    Task<PersonDTO?> GetPersonByPersonalIdAsync(string personalId, CancellationToken cancellationToken);
    Task<PersonDTO?> GerPersonDetailByIdAsync(int id, CancellationToken cancellationToken);
    Task<PersonDTO?> GerPersonByIdAsync(int id, CancellationToken cancellationToken);
    Task<CityDTO?> GerCityIdAsync(int cityId, CancellationToken cancellationToken);
    void UpdatePerson(PersonDTO entity);
    void DeletePersonRelation(PersonRelationDTO entity);
    void DeletePerson(PersonDTO entity);

    Task<PagedResult<PersonDTO>> SearchPersonsAsync(string? quickSearch,
        string? firstName,
        string? lastName,
        string? personalId,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken);

    Task<IQueryable<PersonRelationReportModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken);
}