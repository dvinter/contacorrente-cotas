using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Mappings;
using AlesCotas.Application.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlesCotas.Infra.IoC
{
    public static class DepencyInjectionWebAPI
    {
        // O primeiro parametro define o tipo no qual o metodo está operando
        public static IServiceCollection AddInfrastructureWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Serviços Domain
            services.AddScoped<IProdutoTipoServico, ProdutoTipoServico>();
            services.AddScoped<IProdutoServico, ProdutoServico>();
            services.AddScoped<ISolicitacaoServico, SolicitacaoServico>();
            services.AddScoped<ISolicitacaoItemServico, SolicitacaoItemServico>();

            // Serviços
            services.AddTransient<IProdutoTipoAppServico, ProdutoTipoAppServico>();
            services.AddTransient<IProdutoAppServico, ProdutoAppServico>();
            services.AddTransient<ISolicitacaoAppServico, SolicitacaoAppServico>();

            // AutoMapper
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
