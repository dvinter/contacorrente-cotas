using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AlesCotas.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Presentation.Controllers.Base;
using AlesCotas.Application.Dto;
using AlesCotas.Domain.Enum;
using AlesCotas.Application.Servicos;
using AlesCotas.Application.Filter;
using System.Net;
using System.Net.Http.Headers;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;
using AlesCotas.Domain.Model;
using AlesCotas.Presentation.Models;
using Microsoft.EntityFrameworkCore;

namespace AlesCotas.MVC.Controllers.Users
{
    public class UsersController : BaseController<Usuario, UsuarioDto, UsuarioFilter>
    {
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly IGabineteAppService _gabineteAppService;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILdapAppService _ldapAppService;

        public UsersController(IUsuarioAppService usuarioAppService, IGabineteAppService gabineteAppService,
                               UserManager<Usuario> userManager, IEmailSender emailSender, IConfiguration configuration, ILdapAppService ldapAppService) 
        : base(usuarioAppService, userManager)
        {
            _usuarioAppService = usuarioAppService;
            _gabineteAppService = gabineteAppService;
            _configuration = configuration;
            _emailSender = emailSender;
            _ldapAppService = ldapAppService;
        }

        [HttpGet]
        public async Task<IActionResult> SendChangePasswordEmailAction(string email)
        {
            return Ok(await SendChangePasswordEMail(email));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(Usuario entity)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");

            if (TempData.ContainsKey("toastError"))
                TempData.Remove("toastError");


            try
            {

                if (ModelState.IsValid)
                {
                    if (entity.Id <= 0)
                    {

                        var checkExists = await _usuarioAppService.QueryDto(x => x.UserName.Equals(entity.UserName)).FirstOrDefaultAsync();

                        if (checkExists != null)
                        {
                            await LoadViewBagEditMode(entity);
                            ViewBag.toast = new ToastModel { Error = "Usuário já está cadastrado. Status: " + (checkExists.Ativo ? "Ativo" : "Inativo") };
                            return View(nameof(Create), entity);

                        }
                        else
                        {
                            entity.Ativo = true;
                            var result = await _usuarioAppService.Cadastrar(entity);

                            if (result != null && result.Id > 0)
                            {
                                if (!String.IsNullOrWhiteSpace(entity.Email))
                                {
                                    await this.SendChangePasswordEMail(entity);
                                }
                                TempData.Add("toastSuccess", "Registro inserido com sucesso!");
                            }
                        }
                    }
                    else
                    {
                        var result = await _usuarioAppService.Atualizar(entity);
                        TempData.Add("toastSuccess", "Registro atualizado com sucesso!");
                    }

                    return RedirectToAction(nameof(Index));
                }

                await LoadViewBagEditMode(entity);
                return View(nameof(Create), entity);

            }
            catch (Exception ex)
            {

                await LoadViewBagEditMode(entity);
                ViewBag.toast = new ToastModel { Error = "Erro: " + ex.Message };
                return View(nameof(Create), entity);

            }
        }

        private async Task<bool> SendChangePasswordEMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return await this.SendChangePasswordEMail(user);
        }

        private async Task<bool> SendChangePasswordEMail(Usuario user)
        {
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return false;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailSender.SendEmailAsync(
                user.Email,
                "Mudança de Senha",
                $"Olá, foi requisitada a mudança de sua senha." +
                $"\n\nCaso a requisição não tenha seido feita por você, entre em contato com a DTI imediatamente." +
                $"\n\nUse este código na página em que você requisitou a mudança:" +
                $"\n\n{code}" +
                $"\n\nAtenciosamente," +
                $"\nDiretoria de Tecologia da Informação - DTI");

            return true;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuscarMatriculaAsync(string consultaMatricula)
        {
            try
            {
                ServidorViewModel usuarioServidor = await GetServidoresAsync(consultaMatricula);

                if (usuarioServidor == null)
                {
                    Usuario usuarioNaoEncontrado = new Usuario()
                    {
                        Nome = "Servidor não encontrado!"
                    };

                    return RedirectToAction(nameof(Create), usuarioNaoEncontrado);
                }


                return RedirectToAction(nameof(Create), new Usuario()
                {
                    Nome = usuarioServidor.NomeServidor,
                    Matricula = usuarioServidor.MatriculaVinculo,
                    Email = usuarioServidor.EmailServidor,
                    Setor = usuarioServidor.Setor
                });
            }
            catch (Exception ex)
            {
                Usuario usuarioNaoEncontrado = new Usuario()
                {
                };


                await LoadViewBagEditMode(usuarioNaoEncontrado);
                ViewBag.toast = new ToastModel { Error = "Erro: " + ex.Message };
                return View(nameof(Create), usuarioNaoEncontrado);
            }

        }

        public async Task<ServidorViewModel> GetServidoresAsync(string consultaMatricula)
        {
            return await _ldapAppService.ConsultaUsuario(consultaMatricula);

            //ServidorViewModel servidorConsulta = await _usuarioAppService.GetServidor(consultaMatricula);

            //if (servidorConsulta.erroConexao)
            //{
            //    string apiAles = _configuration.GetConnectionString("api-ales");
            //    string getServidor = _configuration.GetConnectionString("get-servidor");
            //    string apiAuthorization = _configuration.GetConnectionString("api-authorization");

            //    HttpClient client = new HttpClient();
            //    client.BaseAddress = new Uri(apiAles);
            //    var request = new HttpRequestMessage(HttpMethod.Get, getServidor + consultaMatricula);

            //    var byteArray = new UTF8Encoding().GetBytes(apiAuthorization);
            //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            //    var response = await client.SendAsync(request);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonResult = await response.Content.ReadAsStringAsync();

            //        char[] charsToTrim = { '[', ']' };

            //        servidorConsulta = JsonConvert.DeserializeObject<ServidorViewModel>(jsonResult.Trim(charsToTrim));
            //    }
            //}


            //if (servidorConsulta != null)
            //{
            //    var usuario = new Usuario
            //    {
            //        Ativo = true,
            //        Email = servidorConsulta.EmailServidor,//primeiroNome.ToLower() + "@al.es.gov.br",
            //        Nome = servidorConsulta.NomeServidor,
            //        Role = EUsuarioPerfil.Gabinete,
            //        Setor = servidorConsulta.Setor,
            //        Matricula = servidorConsulta.MatriculaVinculo,
            //        GabineteId = null
            //    };

            //    return usuario;
            //}

            //return null;
        }

        public async Task<IActionResult> Enable(int id)
        {
            var result = await _usuarioAppService.SetStatus(id, true);
            TempData["toastSuccess"] = "Usuário habilitado com sucesso!";
            ViewBag.toast = new ToastModel { Success = TempData["toastSuccess"].ToString() };
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Disable(int id)
        {
            var result = await _usuarioAppService.SetStatus(id, false);
            TempData["toastSuccess"] = "Usuário desabilitado com sucesso!";
            ViewBag.toast = new ToastModel { Success = TempData["toastSuccess"].ToString() };
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var result = await _usuarioAppService.Apagar(id);
            return RedirectToAction(nameof(Index));
        }

        protected override async Task LoadViewBagEditMode(Usuario entity = null, bool cleanModelState = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;

            await base.LoadViewBagEditMode(entity, cleanModelState);
            ViewBag.gabinetes = await _gabineteAppService.ListPag(new GabineteFilter { PageNumber = 1, PageSize = int.MaxValue, Status = entity?.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null });
        }




    }
}
