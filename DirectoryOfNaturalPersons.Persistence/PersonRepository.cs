using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Interface;

namespace DirectoryOfNaturalPersons.Persistence;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PersonRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertAsync(PersonDTO entity, CancellationToken cancellationToken)
    {
        await _dbContext.Persons.AddAsync(entity, cancellationToken);
    }
}