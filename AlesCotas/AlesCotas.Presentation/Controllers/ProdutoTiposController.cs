using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Domain.Entities;
using AlesCotas.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlesCotas.Presentation.Controllers
{
    public class ProdutoTiposController : BaseController<ProdutoTipo,ProdutoTipoDto, ProdutoTipoFilter>
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUsuarioAppService _usuarioAppService;
        public ProdutoTiposController(IProdutoTipoAppService produtoAppService, IUsuarioAppService usuarioAppService, IWebHostEnvironment webHostEnvironment, UserManager<Usuario> userManager) : base(produtoAppService,userManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _usuarioAppService = usuarioAppService;
        }


        public override Task<IActionResult> Save(ProdutoTipo entity)
        {
            
            entity.Campos = entity.Campos.Where(x => !String.IsNullOrWhiteSpace(x.Name)).ToList();
            ModelState.Clear();
            TryValidateModel(entity);
            return base.Save(entity);
        }

        protected override async Task LoadViewBagEditMode(ProdutoTipo entity = null, bool cleanModelState = false)
        {
            await base.LoadViewBagEditMode(entity,cleanModelState);
            string pathIcons = Path.Combine(_webHostEnvironment.WebRootPath, "imgs/icons");
            var dir = new DirectoryInfo(pathIcons);
            var iconsInfo = dir.GetFiles();
            ViewBag.icons = iconsInfo.Select(x=>x.Name);
            var listaGestores = await _usuarioAppService.ListPag(new UsuarioFilter { Perfil = Domain.Filtros.Enum.EFiltroUsuarioPerfil.Gestor, Status = entity?.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageSize = int.MaxValue });
            var listOptions = listaGestores.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Nome, Disabled = !x.Ativo }).ToList();
            ViewBag.gestores = listOptions;

        }

    }
}
