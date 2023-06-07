using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class CustomSignInManager : SignInManager<Usuario>
    {
        private readonly ILdapAppService _ldapAppService;
        public CustomSignInManager(UserManager<Usuario> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<Usuario> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<Usuario>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<Usuario> confirmation,
            ILdapAppService ldapAppService)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _ldapAppService = ldapAppService;
        }

        // Implemente métodos personalizados ou substitua os métodos existentes conforme necessário
        public override async Task<SignInResult> PasswordSignInAsync(Usuario user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            // Implemente a lógica personalizada do método PasswordSignInAsync aqui
            // Exemplo: verificação de autenticação baseada em algum critério personalizado
            if (user.UserName.ToLower() == "admin" || !_ldapAppService.IsLdapEnabled())
            {
                return await base.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
            }
            else if (await _ldapAppService.ValidarCredenciais(user.UserName, password))
            {
                // Realize o login do usuário
                await SignInAsync(user, isPersistent);
                return SignInResult.Success;
            }
            else
            {
                // A autenticação falhou
                return SignInResult.Failed;
            }
        }
    }
}
