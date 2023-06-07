using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    sealed internal class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            // P.K
            builder.HasKey(p => p.Id);

            // Propriedades
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");
            builder.Property(p => p.Valor).HasPrecision(8, 2).IsRequired();
            builder.Property(p => p.Unidade).HasMaxLength(5).IsRequired();
            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();

            // Relacionamentos
            builder.HasOne(e => e.Tipo)
                .WithMany()
                .HasForeignKey(e => e.TipoId)
                .IsRequired();
        }
    }
}
