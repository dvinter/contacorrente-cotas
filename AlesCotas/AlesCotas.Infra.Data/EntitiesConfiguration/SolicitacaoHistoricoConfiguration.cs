using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class SolicitacaoHistoricoConfiguration : IEntityTypeConfiguration<SolicitacaoHistorico>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoHistorico> builder)
        {
            // P.K
            builder.HasKey(p => new { p.Sequencia, p.SolicitacaoId });

            // Propriedades
            builder.Property(p => p.DataRegistro).IsRequired();
            builder.Property(p => p.UsuarioRegistroId).IsRequired();
            builder.Property(p => p.Status).IsRequired();

           

            builder.HasOne(e => e.UsuarioRegistro)
                .WithMany()
                .HasForeignKey(e => e.UsuarioRegistroId)
                .IsRequired();
        }
    }
}
