using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class GabineteConfiguration : IEntityTypeConfiguration<Gabinete>
    {
        public void Configure(EntityTypeBuilder<Gabinete> builder)
        {

            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Name).HasMaxLength(150).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");

            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
        }
    }
}
