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
using AlesCotas.Application.Filter;

namespace AlesCotas.Presentation.Controllers
{
    public class GabinetesController : BaseController<Gabinete,GabineteDto, GabineteFilter>
    {
        //IParlamentarAppService _parlamentarAppService;
        public GabinetesController(IGabineteAppService appService , /*IParlamentarAppService parlamentarAppService,*/ UserManager<Usuario> userManager) : base(appService, userManager)
        {
            //_parlamentarAppService = parlamentarAppService;
        }

        protected override async Task LoadViewBagEditMode(Gabinete entity = null, bool cleanModelState = false)
        {
            //ViewBag.parlamentares = await _parlamentarAppService.ListPag(new BaseEntityFilter { Status = entity != null && entity.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            await base.LoadViewBagEditMode(entity,cleanModelState);
        }
    }
}
