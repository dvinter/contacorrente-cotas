using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Domain.Entities;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Presentation.Controllers.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Servicos;
using Microsoft.AspNetCore.Identity;
using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Auth;
using AlesCotas.Presentation.Models;

namespace AlesCotas.Presentation.Controllers
{
    public class SolicitacaoAdiantamentoCotasController : BaseController<SolicitacaoAdiantamentoCota, SolicitacaoAdiantamentoCotaDto, SolicitacaoAdiantamentoCotaFilter>
    {
        private readonly IGabineteAppService _gabineteAppService;
        private readonly ICotaAppService _cotaAppService;
        private readonly ISolicitacaoAdiantamentoCotaAppService _solAdAppService;



        public SolicitacaoAdiantamentoCotasController(IGabineteAppService gabineteAppService, ISolicitacaoAdiantamentoCotaAppService appService, ICotaAppService cotaAppService, UserManager<Usuario> userManager) : base(appService, userManager)
        {
            _solAdAppService = appService;
            _gabineteAppService = gabineteAppService;
            _cotaAppService = cotaAppService;
        }

        protected override async Task LoadViewBagEditMode(SolicitacaoAdiantamentoCota entity = null, bool cleanModelState = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            var gabinetes = await _gabineteAppService.ListPag(new GabineteFilter { GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null, Status = entity != null && entity.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            ViewBag.gabinetes = gabinetes;

            var cota = await _cotaAppService.SelectCotaAtual(DateTime.Now);

            if (cota == null)
            {
                RedirectToAction(nameof(Index));
            }
            ViewBag.cota = cota;
          
            await base.LoadViewBagEditMode(entity,cleanModelState);
        }

        protected override async Task LoadViewBagListMode(SolicitacaoAdiantamentoCotaFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            var gabinetes = await _gabineteAppService.ListPag(new GabineteFilter { GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null, Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            ViewBag.gabinetes = gabinetes;

            await base.LoadViewBagListMode(filter);
        }

        public override async Task<bool> PodeEditar(SolicitacaoAdiantamentoCota entity)
        {
            return await base.PodeEditar(entity);
        }

        [ClaimRequirement(EUsuarioPerfil.Administrador)]

        public async Task<IActionResult> Aprovar(int id)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");

            if (TempData.ContainsKey("toastError"))
                TempData.Remove("toastError");


            try
            {
                if (await _solAdAppService.Aprovar(id))
                {
                    TempData.Add("toastSuccess", "Solicitação aprovada com sucesso.");
                }
                else
                {
                    TempData.Add("toastError", "Não foi possível aprovar a solicitação");
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData.Add("toastError", "Erro: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }

        }


        [ClaimRequirement(EUsuarioPerfil.Administrador)]

        public async Task<IActionResult> Recusar(int id)
        {
            try
            {
                if (await _solAdAppService.Recusar(id))

                {
                    TempData.Add("toastSuccess", "Solicitação recusada com sucesso.");
                }
                else
                {
                    TempData.Add("toastError", "Não foi possível recusar a solicitação");
                }

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData.Add("toastError", "Erro: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
