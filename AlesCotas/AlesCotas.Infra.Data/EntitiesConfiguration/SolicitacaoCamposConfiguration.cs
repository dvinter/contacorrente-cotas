using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class SolicitacaoCamposConfiguration : IEntityTypeConfiguration<SolicitacaoCampos>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoCampos> builder)
        {
            // P.K
            builder.HasKey(p => new { p.Name, p.SolicitacaoId } );

            // Propriedades
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
            builder.Property(p => p.Valor).UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
        }
    }
}
