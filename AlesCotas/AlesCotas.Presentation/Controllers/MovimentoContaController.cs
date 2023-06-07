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
using Microsoft.AspNetCore.Identity;
using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Auth;

namespace AlesCotas.Presentation.Controllers
{
    public class MovimentoContaController : BaseController<MovimentoConta, MovimentoContaDto, BaseEntityFilter>
    {
        IGabineteAppService _gabineteAppService;
        public MovimentoContaController(IMovimentoContaAppService appService, IGabineteAppService gabineteAppService, UserManager<Usuario> userManager) : base(appService, userManager)
        {
            _gabineteAppService = gabineteAppService;
        }

        [ClaimRequirement(EUsuarioPerfil.Administrador, EUsuarioPerfil.Gestor)]
        public override async Task<IActionResult> Save(MovimentoConta entity)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");

            if (ModelState.IsValid)
            {
                if (entity.Id <= 0)
                {
                    var result = await _appServico.Cadastrar(entity);
                    TempData.Add("toastSuccess", "Registro inserido com sucesso!");
                }
                else
                {
                    var result = await _appServico.Atualizar(entity);
                    TempData.Add("toastSuccess", "Registro atualizado com sucesso!");
                }

                return RedirectToAction(nameof(Index),"Extrato");
            }
            await LoadViewBagEditMode(entity);
            return View(nameof(Create), entity);
        }

        protected override async Task LoadViewBagEditMode(MovimentoConta entity = null, bool cleanModelState = false)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            var gabinetes = await _gabineteAppService.ListPag(new Application.Filter.GabineteFilter { GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null, Status = entity != null && entity.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            ViewBag.gabinetes = gabinetes;
            await base.LoadViewBagEditMode(entity,cleanModelState);
        }
    }
}
