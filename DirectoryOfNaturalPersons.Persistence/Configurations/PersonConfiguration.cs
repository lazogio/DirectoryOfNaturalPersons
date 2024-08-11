using DirectoryOfNaturalPersons.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryOfNaturalPersons.Persistence.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<PersonDTO>
{
    public void Configure(EntityTypeBuilder<PersonDTO> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.PersonalId).HasMaxLength(11);
        builder.HasMany(x => x.PhoneNumbers)
            .WithOne(x => x.PersonDto)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RelatedPersons)
            .WithOne(x => x.PersonDto)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.RelatedToPersons)
            .WithOne(x => x.RelatedPersonDto)
            .HasForeignKey(x => x.RelatedPersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}