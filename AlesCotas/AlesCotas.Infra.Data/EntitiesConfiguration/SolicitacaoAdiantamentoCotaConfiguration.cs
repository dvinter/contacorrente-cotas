using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class SolicitacaoAdiantamentoCotaConfiguration : IEntityTypeConfiguration<SolicitacaoAdiantamentoCota>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoAdiantamentoCota> builder)
        {

            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Descricao).HasMaxLength(200).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");

            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
        }
    }
}
