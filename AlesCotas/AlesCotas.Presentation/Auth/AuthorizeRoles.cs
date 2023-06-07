using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AlesCotas.Presentation.Auth
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(params EUsuarioPerfil[] roles) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] {
                    roles
            };
        }
    }
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly EUsuarioPerfil[] roles;
        private readonly ILogger<ClaimRequirementFilter> logger;
        public ClaimRequirementFilter(EUsuarioPerfil[] roles, ILogger<ClaimRequirementFilter> logger)
        {
            this.roles = roles;
            this.logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var email = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
            var login = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;

            var userRoles = context.HttpContext.User.Claims.Where(x => x.Type.Equals(ClaimTypes.Role)).Select(v=> Enum.Parse<EUsuarioPerfil>(v.Value)).ToList();

            if (userRoles == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (!roles.Intersect(userRoles).Any())
            {
                this.logger.LogInformation($"Falha na autorização: Usuário sem permissão a este recurso. Usuário: {email}/{login} Recurso: " + context.HttpContext.Request.Method + ": " + context.HttpContext.Request.Path);
                context.Result = new ForbidResult();
                return;

            }
            
        }


    }
}
