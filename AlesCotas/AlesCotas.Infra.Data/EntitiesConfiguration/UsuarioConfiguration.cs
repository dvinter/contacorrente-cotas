using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // P.K
            builder.HasKey(e => e.Id);
            builder.ToTable("Usuarios");
            // Propriedades
            builder.Property(p => p.Nome).HasMaxLength(150).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");

            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            //builder.Property(p => p.UpdatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();

            builder.HasIndex(p => p.Email).IsUnique();
            builder.HasIndex(p => p.NormalizedEmail).IsUnique();

            // Relacionamentos
            builder.HasOne(e => e.Gabinete)
                .WithMany()
                .HasForeignKey(e => e.GabineteId);

            builder.Property(p => p.ResetPassToken);
            builder.Property(p => p.ResetPassTokenExpire);
            builder.Property(p => p.Attempts);



        }
    }
}
