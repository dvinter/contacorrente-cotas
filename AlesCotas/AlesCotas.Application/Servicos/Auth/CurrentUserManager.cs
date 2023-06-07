using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Infra.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AlesCotas.Application.Servicos.Auth
{
    /// <summary>
    /// Classe utilizada para obter dados do usuário logado
    /// </summary>
    public class CurrentUserManager : ICurrentUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Usuario> _userManager;
        public CurrentUserManager(IHttpContextAccessor contextAccessor, UserManager<Usuario> userManager)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<Usuario> GetCurrentUser()
        {
            var user = _contextAccessor.HttpContext?.User;
            if (user == null)
            {
                var systemUser = await GetUser("system");
                if (systemUser == null)
                {
                    return null;
                }
                else
                    return systemUser;
            }

            var currentUser = await this._userManager.GetUserAsync(user);
            var roles = (await this._userManager.GetRolesAsync(currentUser)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>();
            currentUser.Role = roles?.Min() ?? EUsuarioPerfil.Consulta;
            return currentUser;
        }

        public async Task<Usuario> GetUser(string name)
        {
            var currentUser = await this._userManager.FindByNameAsync(name);
            if (currentUser == null)
                return null;

            var roles = (await this._userManager.GetRolesAsync(currentUser)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>();
            currentUser.Role = roles.Any() ?  roles?.Min() ?? EUsuarioPerfil.Consulta : null;
            return currentUser;
        }

        public string GetRemoteAddress()
        {
            if (_contextAccessor.HttpContext is null)
                return "::1";

            string ip =  _contextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();

            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                string forwardedFor = _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];

                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    ip = forwardedFor.Split(',')[0];
                }
            }

            return ip;
        }
    }
}
