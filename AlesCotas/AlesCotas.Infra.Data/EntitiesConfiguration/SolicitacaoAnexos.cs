using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class SolicitacaoAnexosConfiguration : IEntityTypeConfiguration<SolicitacaoAnexos>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoAnexos> builder)
        {

            // P.K
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(255);
            builder.Property(p => p.Extensao).HasMaxLength(6);
          
        }
    }
}
