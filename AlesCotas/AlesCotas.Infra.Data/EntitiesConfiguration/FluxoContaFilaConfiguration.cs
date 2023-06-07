using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class FluxoContaFilaConfiguration : IEntityTypeConfiguration<FluxoContaFila>
    {
        public void Configure(EntityTypeBuilder<FluxoContaFila> builder)
        {
            builder.ToTable("fluxo_conta_fila");
            // P.K
            builder.HasKey(p => p.Id);
            // Propriedades
           
        }
    }
}
