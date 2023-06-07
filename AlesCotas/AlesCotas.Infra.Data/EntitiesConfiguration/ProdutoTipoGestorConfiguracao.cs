using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class ProdutoTipoGestorConfiguracao : IEntityTypeConfiguration<ProdutoTipoGestor>
    {
        public void Configure(EntityTypeBuilder<ProdutoTipoGestor> builder)
        {
            // P.K
            builder.HasKey(p => new { p.ProdutoTipoId, p.UsuarioId });

            // Propriedades
            builder.HasOne(x=>x.Usuario).WithMany().HasForeignKey(x=>x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            
        }
    }
   
}
