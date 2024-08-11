using DirectoryOfNaturalPersons.Domain.Entities;
using DirectoryOfNaturalPersons.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DirectoryOfNaturalPersons.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<PersonDTO> Persons { get; set; }
    public DbSet<PersonRelationDTO> PersonRelations { get; set; }
    public DbSet<PhoneNumberDTO> PhoneNumbers { get; set; }
    public DbSet<CityDTO> Cities { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PersonConfiguration());
        modelBuilder.ApplyConfiguration(new PersonRelationConfiguration());
        modelBuilder.ApplyConfiguration(new PhoneNumberConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
    }
}