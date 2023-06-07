using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class FluxoContaConfiguration : IEntityTypeConfiguration<FluxoConta>
    {
        public void Configure(EntityTypeBuilder<FluxoConta> builder)
        {
            builder.ToTable("fluxo_conta");

            // P.K
            builder.HasKey(p => new { p.GabineteId, p.Sequencia });
            builder.HasIndex(p => new { p.MovimentoId, p.SequenciaMovimento }).IsUnique();
            builder.HasIndex(p => new { p.Ano, p.Mes });
      
        }
    }
}
