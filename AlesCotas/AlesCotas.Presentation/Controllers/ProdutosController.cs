using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Domain.Entities;
using AlesCotas.Presentation.Controllers.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlesCotas.Presentation.Controllers
{
    public class ProdutosController : BaseController<Produto,ProdutoDto, ProdutoFilter>
    {
        private readonly IProdutoTipoAppService _produtoTipoAppService;
        public ProdutosController(IProdutoAppService produtoAppService, IProdutoTipoAppService produtoTipoAppService, UserManager<Usuario> userManager) : base(produtoAppService,userManager)
        {
            _produtoTipoAppService = produtoTipoAppService;
        }
        public async Task<IActionResult> SelectTipos(ProdutoTipoFilter? filter = null)
        {
            if (filter == null)
                filter = new ProdutoTipoFilter { PageSize = 12, PageNumber = 1 };

            ViewBag.filtro = filter;
            var lista = await _produtoTipoAppService.ListPag(filter);
            return View(lista);
        }
        protected override async Task LoadViewBagEditMode(Produto entity = null, bool cleanModelState = false)
        {
            if (entity?.TipoId > 0)
            {
                ViewBag.produtoTipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(entity.TipoId));
                await base.LoadViewBagEditMode(entity,cleanModelState);
            }
            else
                RedirectToAction("SelectTipos", nameof(ProdutoTiposController));
        }


        protected override async Task LoadViewBagListMode(ProdutoFilter filter)
        {
            if (filter.ProdutoTipoId > 0)
                ViewBag.produtoTipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(filter.ProdutoTipoId));
            await base.LoadViewBagListMode(filter);
        }

        public override async Task<IActionResult> Save(Produto entity)
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

                return RedirectToAction(nameof(Index), new { ProdutoTipoId=entity.TipoId });
            }
            await LoadViewBagEditMode(entity);
            return View(nameof(Create), entity);
        }

    }
}
