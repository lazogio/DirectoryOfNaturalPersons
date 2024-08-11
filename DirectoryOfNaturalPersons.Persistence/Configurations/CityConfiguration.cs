using DirectoryOfNaturalPersons.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DirectoryOfNaturalPersons.Persistence.Configurations;

public class CityConfiguration : IEntityTypeConfiguration<CityDTO>
{
    public void Configure(EntityTypeBuilder<CityDTO> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NameKa).HasMaxLength(30);
        builder.Property(x => x.NameEn).HasMaxLength(30);
    }
}