using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class SolicitacaoConfiguration : IEntityTypeConfiguration<Solicitacao>
    {
        public void Configure(EntityTypeBuilder<Solicitacao> builder)
        {

            // P.K
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.ProdutoTipo).WithMany().HasForeignKey(p => p.ProdutoTipoId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Campos).WithOne().HasForeignKey(p => p.SolicitacaoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Historico).WithOne().HasForeignKey(p => p.SolicitacaoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Items).WithOne().HasForeignKey(p => p.SolicitacaoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Anexos).WithOne().HasForeignKey(p => p.SolicitacaoId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.Numero).IsUnique();
            builder.Property(p => p.Numero).HasMaxLength(20);
          
        }
    }
}
