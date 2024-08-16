using DirectoryOfNaturalPersons.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryOfNaturalPersons.Persistence.Configurations;

public class PersonRelationConfiguration : IEntityTypeConfiguration<PersonRelationDTO>
{
    public void Configure(EntityTypeBuilder<PersonRelationDTO> builder)
    {
        builder.HasKey(x => new { x.PersonId, x.RelatedPersonId });
        builder.HasOne(x => x.PersonDto)
            .WithMany(x => x.RelatedPersons)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.RelatedPersonDto)
            .WithMany()
            .HasForeignKey(x => x.RelatedPersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}