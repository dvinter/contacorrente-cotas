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

namespace AlesCotas.Presentation.Controllers
{
    public class CotasController : BaseController<Cota,CotaDto, BasePeriodoFilter>
    {
        private readonly ICotaAppService _cotaAppService;
        public CotasController(ICotaAppService appService, UserManager<Usuario> userManager) : base(appService,userManager)
        {
            _cotaAppService = appService;
        }


        public async Task<IActionResult> ProcessarCotas()
        {
            var i = await _cotaAppService.ProcessarCotas();
            return Ok(i);
        }

        protected override async Task LoadViewBagEditMode(Cota entity = null, bool cleanModelState = false)
        {
            var ultimaCota = await _cotaAppService.QueryDto().OrderByDescending(x => x.DataInicio).Take(1).FirstOrDefaultAsync();
            ViewBag.minDate =  ultimaCota != null ? ultimaCota.DataInicio.AddDays(entity?.Id > 0 ? 0 : 1) : DateTime.MinValue;
            await base.LoadViewBagEditMode(entity,cleanModelState);
        }
    }
}
