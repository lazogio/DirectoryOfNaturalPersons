using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.Interface;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PersonDTO?> GetPersonByPersonalIdAsync(string personalId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Persons
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PersonalId == personalId, cancellationToken)
            .ConfigureAwait(false);
    }
}