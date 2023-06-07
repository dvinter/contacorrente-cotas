using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Model;
using AlesCotas.Infra.Data.SqlQuery;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlesCotas.Infra.Data.Context
{
    

    public class ApplicationDbContext : IdentityDbContext<Usuario, Roles, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public ApplicationDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

       
        public DbSet<ProdutoTipo> ProdutoTipo { get; set; }
        public DbSet<ProdutoTipoUsuarioPerfil> ProdutoTipoUsuarioPerfils { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Solicitacao> Solicitacao { get; set; }
        public DbSet<SolicitacaoHistorico> SolicitacaoHistorico { get; set; }
        public DbSet<SolicitacaoItem> SolicitacaoItem { get; set; }
        public DbSet<SolicitacaoAnexos> SolicitacaoAnexos { get; set; }
        public DbSet<Cota> Cotas { get; set; }
        public DbSet<Gabinete> Gabinetes { get; set; }
       
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<FluxoContaFila> FluxoContaFila { get; set; }
        public DbSet<FluxoConta> FluxoConta { get; set; }
        public DbSet<MovimentoConta> MovimentoConta { get; set; }
        public DbSet<SolicitacaoAdiantamentoCota> SolicitacaoAdintamentoCota { get; set; }
        public DbSet<SolicitacaoCampos> SolicitacaoCampos { get; set; }

        public DbSet<Publicacao> Publicacoes { get; set; }

        protected override void ConfigureConventions(
    ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 2);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<ExtratoModel>().HasNoKey().ToSqlQuery(SqlQueryExtratoView.QUERY_FLUXO_DATA_MOVIMENTO);
            modelBuilder.Entity<TotalSolicitacoesModel>().HasNoKey().ToSqlQuery(SqlQueryTotalSolicitacoesView.QUERY);
            modelBuilder.Entity<ExtratoTotalModel>().HasNoKey().ToSqlQuery(SqlQueryExtratoView.QUERY_TOTAIS);

        }
    }
}
