using DirectoryOfNaturalPersons.Domain.Entities;

namespace DirectoryOfNaturalPersons.Domain.Interface;

public interface IPersonRepository
{
    Task InsertAsync(PersonDTO entity, CancellationToken cancellationToken);
}