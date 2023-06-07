using AlesCotas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace AlesCotas.Infra.Data.EntitiesConfiguration
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            // P.K
            builder.HasKey(e => e.Id);
            builder.ToTable("Logs");
            // Propriedades
            builder.Property(p => p.Description).HasMaxLength(800).IsRequired().UseCollation("SQL_LATIN1_GENERAL_CP1_CI_AI");

            // Datas
            //builder.Property(p => p.CreatedAt).HasColumnType("datetime2").HasPrecision(0).IsRequired();
            builder.Property(p => p.EventDate).HasColumnType("datetime2").HasPrecision(0).IsRequired().HasDefaultValueSql("getdate()");

            builder.HasIndex(p => new { p.Entity, p.RefId });
            builder.HasIndex(p => p.EventDate);

            // Relacionamentos
            builder.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
