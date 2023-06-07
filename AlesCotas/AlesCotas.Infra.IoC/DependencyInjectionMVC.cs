using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Mappings;
using AlesCotas.Application.Servicos;
using AlesCotas.Domain.Conta;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlesCotas.Infra.IoC
{
    public static class DependencyInjectionMVC
    {
        // O primeiro parametro define o tipo no qual o metodo está operando
        public static IServiceCollection AddInfrastructureMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            });


           

            //// Identity
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/";
            //});

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Serviços Domain
            services.AddScoped<IProdutoTipoServico, ProdutoTipoServico>();
            services.AddScoped<IProdutoServico, ProdutoServico>();
            services.AddScoped<ISolicitacaoServico, SolicitacaoServico>();
            services.AddScoped<ISolicitacaoItemServico, SolicitacaoItemServico>();
            services.AddScoped<ISolicitacaoItemArquivoServico, SolicitacaoItemArquivoServico>();

            // Serviços Application
            services.AddTransient<IProdutoTipoAppServico, ProdutoTipoAppServico>();
            services.AddTransient<IProdutoAppServico, ProdutoAppServico>();
            services.AddTransient<ISolicitacaoAppServico, SolicitacaoAppServico>();
            services.AddTransient<ISolicitacaoItemArquivoAppServico, SolicitacaoItemArquivoAppServico>();

            // AutoMapper
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            // Identity
            //services.AddScoped<IAutenticar, AutenticarServico>();
            services.AddScoped<IUsuarioRolesInicial, UsuarioRolesInicial>();

            return services;
        }
    }
}
