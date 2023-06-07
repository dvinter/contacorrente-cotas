using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class PublicacaoConfiguration : IEntityTypeConfiguration<Publicacao>
    {
        public void Configure(EntityTypeBuilder<Publicacao> builder)
        {
            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Descricao).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");

            // Datas
            builder.Property(p => p.MesReferencia).HasColumnType("datetime2").HasPrecision(0).IsRequired();

            builder.HasIndex(x => new { x.MesReferencia, x.GabineteId }).IsUnique();

            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
        }
    }
}
