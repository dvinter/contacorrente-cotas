using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class CotaConfiguration : IEntityTypeConfiguration<Cota>
    {
        public void Configure(EntityTypeBuilder<Cota> builder)
        {

            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Descricao).HasMaxLength(200).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
            builder.Property(p => p.Lei).HasMaxLength(150).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
            builder.HasIndex(p => p.DataInicio).IsUnique();

            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
        }
    }
}
