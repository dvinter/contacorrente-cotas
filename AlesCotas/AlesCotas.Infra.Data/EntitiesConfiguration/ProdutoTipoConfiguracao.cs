using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class ProdutoTipoConfiguracao : IEntityTypeConfiguration<ProdutoTipo>
    {
        public void Configure(EntityTypeBuilder<ProdutoTipo> builder)
        {
            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
           
            builder.Property(p => p.Ativo).IsRequired();
            builder.HasMany(x => x.Campos).WithOne().HasForeignKey(x => x.ProdutoTipoId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Gestores).WithOne().HasForeignKey(x => x.ProdutoTipoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
    sealed internal class ProtudoTipoCamposConfiguracao : IEntityTypeConfiguration<ProdutoTipoCampos>
    {
        public void Configure(EntityTypeBuilder<ProdutoTipoCampos> builder)
        {
            builder.ToTable("ProdutoTipoCampos");
            // P.K
            builder.HasKey(p =>  p.Id);
            
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
            builder.HasIndex(p => new { p.ProdutoTipoId, p.Name }).IsUnique();
       
        }
    }
    sealed internal class ProdutoTipoUsuarioPerfilConfiguracao : IEntityTypeConfiguration<ProdutoTipoUsuarioPerfil>
    {
        public void Configure(EntityTypeBuilder<ProdutoTipoUsuarioPerfil> builder)
        {
            builder.ToTable("ProdutoTipoUsuarioPerfil");
            // P.K
            builder.HasKey(p => new { p.ProdutoTipoId, p.Perfil });

           
        }
    }
}
