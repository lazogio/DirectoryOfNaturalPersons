using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Domain.GenericModel;
using DirectoryOfNaturalPersons.Domain.Interface;
using DirectoryOfNaturalPersons.Domain.Models;
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

    public async Task<PersonDTO?> GerPersonDetailByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Persons
            .Include(rp => rp.RelatedPersons)
            .ThenInclude(p => p.RelatedPersonDto)
            .Include(p => p.PhoneNumbers)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<PersonDTO?> GerPersonByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Persons.FindAsync(id, cancellationToken);
    }

    public async Task<CityDTO?> GerCityIdAsync(int cityId, CancellationToken cancellationToken)
    {
        return await _dbContext.Cities.FindAsync(cityId, cancellationToken);
    }

    public void UpdatePerson(PersonDTO entity)
    {
        _dbContext.Persons.Update(entity);
    }

    public void DeletePersonRelation(PersonRelationDTO entity)
    {
        _dbContext.PersonRelations.Remove(entity);
    }

    public void DeletePerson(PersonDTO entity)
    {
        _dbContext.Persons.Remove(entity);
    }

    public async Task<PagedResult<PersonDTO>> SearchPersonsAsync(string? quickSearch, string? firstName,
        string? lastName, string? personalId, int? page, int? pageSize, CancellationToken cancellationToken)
    {
        if (_dbContext is null)
        {
            throw new ArgumentNullException(nameof(_dbContext), @"DbContext cannot be null.");
        }

        IQueryable<PersonDTO> query = _dbContext.Persons
            .Include(p => p.PhoneNumbers);

        query = query
            .Where(p => (string.IsNullOrEmpty(quickSearch) ||
                         EF.Functions.Like(p.FirstName, $"%{quickSearch}%") ||
                         EF.Functions.Like(p.LastName, $"%{quickSearch}%") ||
                         EF.Functions.Like(p.PersonalId, $"%{quickSearch}%"))
            )
            .Where(p => (string.IsNullOrEmpty(firstName) || p.FirstName.Contains(firstName)))
            .Where(p => (string.IsNullOrEmpty(lastName) || p.LastName.Contains(lastName)))
            .Where(p => (string.IsNullOrEmpty(personalId) || p.PersonalId.Contains(personalId)));

        var totalCount = await query.CountAsync(cancellationToken);

        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var results = await query.ToListAsync(cancellationToken);

        return new PagedResult<PersonDTO>
        {
            TotalCount = totalCount,
            Page = page ?? 1,
            PageSize = pageSize ?? results.Count,
            Results = results.Select(x => new PersonDTO
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                PersonalId = x.PersonalId,
                BirthDate = x.BirthDate,
                Gender = x.Gender,
                CityId = x.CityId,
                PhoneNumbers = x.PhoneNumbers.Select(pn => new PhoneNumberDTO()
                {
                    NumberType = pn.NumberType,
                    Number = pn.Number
                }).ToList()
            }).ToList()
        };
    }

    public Task<IQueryable<PersonRelationReportModel>> GetPersonsRelationsAsync(
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.PersonRelations
            .AsNoTracking()
            .Include(p => p.PersonDto)
            .GroupBy(x => new
            {
                x.PersonId,
                x.RelationType,
                x.PersonDto.FirstName,
                x.PersonDto.LastName,
                x.PersonDto.Gender
            })
            .Select(c => new
            {
                c.Key,
                Count = c.Count()
            })
            .OrderBy(m => m.Key.PersonId)
            .ThenBy(m => m.Key.RelationType)
            .Select(s => new PersonRelationReportModel
            {
                Id = s.Key.PersonId,
                FirstName = s.Key.FirstName,
                LastName = s.Key.LastName,
                Gender = s.Key.Gender,
                RelationType = s.Key.RelationType,
                Count = s.Count
            });

        return Task.FromResult(query);
    }
}