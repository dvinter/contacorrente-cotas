using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;

namespace AlesCotas.Presentation.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<Usuario> _userManager;
        public CustomExceptionFilter(ApplicationDbContext applicationDbContext, UserManager<Usuario> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public override void OnException(ExceptionContext context)
        {
            try
            {
                // captura a exceção
                Exception ex = context.Exception;

                string descricao = ex.Message;

                if (ex.InnerException != null)
                {
                    descricao = ex.InnerException.Message;
                }

                if (descricao.Length > 800)
                {
                    descricao = descricao.Substring(0, 800);
                }

                //var controllerTypeInfo = context.ActionDescriptor.ControllerTypeInfo;
                //var controller = (ControllerBase)Activator.CreateInstance(controllerTypeInfo.AsType());

                //// adiciona uma mensagem de erro personalizada à ViewBag
                //controller.ViewBag.ErrorMessage = "Ocorreu um erro ao processar sua solicitação.";

                string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

                if (context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    string forwardedFor = context.HttpContext.Request.Headers["X-Forwarded-For"];

                    if (!string.IsNullOrEmpty(forwardedFor))
                    {
                        ip = forwardedFor.Split(',')[0];
                    }
                }

                string path = context.HttpContext.Request.Path;
                if (path.Length > 450)
                    path = path.Substring(0, 450);


                var user = context.HttpContext.User;
                int? userId = null;
                if (user != null)
                {
                    var currentUser = this._userManager.GetUserAsync(user).Result;
                    if (currentUser != null)
                    {
                        userId = currentUser.Id;
                    }
                }


                _applicationDbContext.Logs.Add(new Domain.Entities.Log { Description = descricao, Entity = path, EventDate = DateTime.Now, IpAddress = ip, RefId = null, EventType = Domain.Enum.ETipoEventoLog.Error, UserId = userId });
                _applicationDbContext.SaveChanges();
                // faça o que for necessário com a exceção capturada, por exemplo:
                // - logá-la
                // - exibir uma página de erro personalizada
                // - redirecionar para uma outra página
            }
            catch
            {
                //
            }
            finally
            {
                base.OnException(context);
            }
            // permite que a exceção seja tratada por outros filtros ou pelo pipeline de middleware
            
        }
    }
}
