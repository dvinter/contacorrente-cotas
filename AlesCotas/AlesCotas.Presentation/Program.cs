using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Mappings;
using AlesCotas.Application.Servicos;
using AlesCotas.Application.Servicos.Auth;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories;
using AlesCotas.Presentation.Worker;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using AlesCotas.Presentation.Filters;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var ptBR = new CultureInfo("pt-BR");
        var enUS = new CultureInfo("en-US");
        ptBR.NumberFormat.NumberDecimalDigits = ptBR.NumberFormat.CurrencyDecimalDigits = 2;

        //Configurado pra usar separadores no padrão americano, pois se no padrão BR, o ASP.NET envia para o front valores numéricos com vírgula e o navegador não identifica como números, mas como texto.
        ptBR.NumberFormat.NumberDecimalSeparator = ptBR.NumberFormat.CurrencyDecimalSeparator = ".";
        ptBR.NumberFormat.NumberGroupSeparator = ptBR.NumberFormat.CurrencyGroupSeparator = ",";


        CultureInfo.DefaultThreadCurrentCulture = ptBR;
        CultureInfo.DefaultThreadCurrentUICulture = ptBR;


        var builder = WebApplication.CreateBuilder(args);

        //Configuração do Entity
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();



        //Configuração do Identity
        builder.Services.AddDefaultIdentity<Usuario>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Tokens.PasswordResetTokenProvider = "CustomTokenProvider";
        })
        .AddRoles<Roles>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddTokenProvider<CustomTokenProvider>("CustomTokenProvider");

        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(typeof(CustomExceptionFilter));


        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.IgnoreNullValues = true;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            options.JsonSerializerOptions.NumberHandling =  JsonNumberHandling.Strict;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.JsonSerializerOptions.AllowTrailingCommas = true;
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        });


        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture(ptBR);
            options.SupportedCultures = new[] {  ptBR, enUS };
            options.SupportedUICultures = new[] { ptBR  , enUS };
        });


        builder.Services.AddTransient<IEmailSender, MailSenderServico>();

        // Unit of Work
        builder.Services.AddScoped<ICurrentUserManager, CurrentUserManager>();
        
        //Repositories
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<IGabineteRepository, GabineteRepository>();
        builder.Services.AddScoped<IProdutoTipoRepository, ProdutoTipoRepository>();
        builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
        builder.Services.AddScoped<IFluxoContaRepository, FluxoContaRepository>();
        builder.Services.AddScoped<ICotaRepository, CotaRepository>();
        builder.Services.AddScoped<IMovimentoContaRepository, MovimentoContaRepository>();
        builder.Services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
        builder.Services.AddScoped<IPublicacaoRepository, PublicacaoRepository>();
        builder.Services.AddScoped<ISolicitacaoAdiantamentoCotaRepository, SolicitacaoAdiantamentoCotaRepository>();
        builder.Services.AddScoped<SignInManager<Usuario>, CustomSignInManager>();

        // Services Application
        builder.Services.AddTransient<IUsuarioAppService, UsuarioAppService>();
        builder.Services.AddTransient<IGabineteAppService, GabineteAppService>();
        builder.Services.AddTransient<IProdutoTipoAppService, ProdutoTipoAppService>();
        builder.Services.AddTransient<IProdutoAppService, ProdutoAppService>();
        builder.Services.AddTransient<ISolicitacaoAppService, SolicitacaoAppService>();
        builder.Services.AddTransient<IFluxoContaAppService, FluxoContaAppService>();
        builder.Services.AddTransient<ICotaAppService, CotaAppService>();
        builder.Services.AddTransient<IMovimentoContaAppService, MovimentoContaAppService>();
        builder.Services.AddTransient<ISolicitacaoAdiantamentoCotaAppService, SolicitacaoAdiantamentoCotaAppService>();
        builder.Services.AddTransient<IPublicacaoAppService, PublicacaoAppService>();
        builder.Services.AddTransient<IDashBoardAppService, DashBoardAppService>();
        builder.Services.AddTransient<ILdapAppService, LdapAppService>();
        

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

        builder.Services.AddHostedService<AlesWorker>();

        var app = builder.Build();

        

        //Executa as migrações do banco que estiverem pendente e criação do usuário admin padrão e Roles padrão
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Roles>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
            var roles = Enum.GetValues(typeof(EUsuarioPerfil));
            IdentityResult result;
            foreach (int role in roles)
            {

                var roleExist = await roleManager.RoleExistsAsync(role.ToString());
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new Roles() { Perfil = (EUsuarioPerfil)role });
                }
            }

            {
                var usrSystem = await userManager.Users.FirstOrDefaultAsync(x => x.UserName.Equals("system"));
                if (usrSystem == null)
                {
                    usrSystem = new Usuario { UserName = "system", Nome = "Sistema", Ativo = false, LockoutEnd = DateTimeOffset.MaxValue, LockoutEnabled = true };
                    result = await userManager.CreateAsync(usrSystem);
                }
            }

            if (!(await userManager.GetUsersInRoleAsync(((int)EUsuarioPerfil.Administrador).ToString())).Any())
            {
                {
                    var admin = await userManager.Users.FirstOrDefaultAsync(x => x.UserName.Equals("admin"));
                    if (admin == null)
                    {
                        admin = new Usuario { UserName = "admin", Nome = "Administrador", Ativo = true };
                        result = await userManager.CreateAsync(admin, "NAknF!$fA!");
                        if (result.Succeeded)
                            await userManager.AddToRoleAsync(admin, ((int)EUsuarioPerfil.Administrador).ToString());

                        await db.Logs.AddAsync(new Log { Description = "Inserção Usuário padrão", Entity = "Usuarios", EventDate = DateTime.Now, EventType = ETipoEventoLog.System, IpAddress = null, UserId = null, RefId = admin.Id });
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(admin, ((int)EUsuarioPerfil.Administrador).ToString());
                        await db.Logs.AddAsync(new Log { Description = "Atualização Usuário padrão", Entity = "Usuarios", EventDate = DateTime.Now, EventType = ETipoEventoLog.System, IpAddress = null, UserId = null, RefId = admin.Id });
                    }
                }

               

                await db.SaveChangesAsync();
            }

        }



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}