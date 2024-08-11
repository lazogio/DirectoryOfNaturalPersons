using DirectoryOfNaturalPersons.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryOfNaturalPersons.Persistence.Configurations;

public class PhoneNumberConfiguration : IEntityTypeConfiguration<PhoneNumberDTO>
{
    public void Configure(EntityTypeBuilder<PhoneNumberDTO> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Number).HasMaxLength(50);
        builder.HasOne(x => x.PersonDto)
            .WithMany(x => x.PhoneNumbers)
            .HasForeignKey(x => x.PersonId);
    }
}