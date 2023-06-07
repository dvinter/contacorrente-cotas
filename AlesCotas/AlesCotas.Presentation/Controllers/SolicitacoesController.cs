using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Models;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Model;
using AlesCotas.Presentation.Auth;
using AlesCotas.Presentation.Controllers.Base;
using AlesCotas.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AlesCotas.Presentation.Controllers
{
    public class SolicitacoesController : BaseController<Solicitacao, SolicitacaoDto, SolicitacaoFilter>
    {
        private readonly IProdutoTipoAppService _produtoTipoAppService;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IGabineteAppService _gabineteAppService;
        private readonly ISolicitacaoAdiantamentoCotaAppService _solicitacaoAdiantamentoCotaAppService;
        private readonly ISolicitacaoAppService _solicitacaoAppService;
        private readonly IMovimentoContaAppService _movimentoContaAppService;
        private readonly ILogger<SolicitacoesController> _logger;

        public SolicitacoesController(ISolicitacaoAppService solicitacaoAppService,
            IGabineteAppService gabineteAppService,
            IProdutoAppService produtoAppService,
            IProdutoTipoAppService produtoTipoAppService,
            ISolicitacaoAdiantamentoCotaAppService solicitacaoAdiantamentoCotaAppService,
            IMovimentoContaAppService movimentoContaAppService,
            ILogger<SolicitacoesController> logger,
            UserManager<Usuario> userManager) : base(solicitacaoAppService, userManager)
        {
            _logger = logger;
            _solicitacaoAppService = solicitacaoAppService;
            _produtoTipoAppService = produtoTipoAppService;
            _produtoAppService = produtoAppService;
            _gabineteAppService = gabineteAppService;
            _solicitacaoAdiantamentoCotaAppService = solicitacaoAdiantamentoCotaAppService;
            _movimentoContaAppService = movimentoContaAppService;
        }
        public async Task<IActionResult> SelectTipos(ProdutoTipoFilter? filter = null)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;

            if (filter == null)
                filter = new ProdutoTipoFilter { PageSize = 12, PageNumber = 1 };

            if (user.Role == EUsuarioPerfil.Gestor)
                filter.FilterExpression = (x => x.Gestores.Any(y=>y.UsuarioId == user.Id));

            ViewBag.filtro = filter;


            ViewBag.user = user;

            var lista = await _produtoTipoAppService.ListPag(filter);

            var qtdSolAd = await _solicitacaoAdiantamentoCotaAppService.Count(x => !x.DataResposta.HasValue);
            ViewBag.qtdSolAd = qtdSolAd;

            return View(lista);
        }


        public override Task<IActionResult> Edit(int? id)
        {
            return base.Edit(id);
        }

        protected override async Task LoadViewBagEditMode(Solicitacao entity = null, bool cleanModelState = false)
        {
            if (entity?.ProdutoTipoId > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                user.Role = role;

                if (user.Role == EUsuarioPerfil.Gabinete)
                {
                    var gabinete = await _gabineteAppService.SelectDto(x => x.Id.Equals(user.GabineteId));
                    ViewBag.gabinetes = new List<GabineteDto>() { gabinete };
                    //model.Gabinetes = new List<NovaSolicitacaoGabinetes>(1);
                    //model.Gabinetes.Add(new NovaSolicitacaoGabinetes { Checked = true, Gabinete = gabinete });                
                }
                else
                {
                    ViewBag.gabinetes = (await _gabineteAppService.ListPag(new GabineteFilter { PageNumber = 1, PageSize = int.MaxValue, Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null })).ToList();
                }

                ViewBag.itens = await _produtoAppService.ListPag(new ProdutoFilter { ProdutoTipoId = entity.ProdutoTipoId, Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, PageSize = int.MaxValue, PageNumber = 1 });

                if (entity.Id >0)
                {
                    var gabinete = await _gabineteAppService.SelectDto(x => x.Id.Equals(entity.GabineteId));
                    ViewBag.gabinete = gabinete;
                }


                //ViewBag.gabinetes = await _gabineteAppService.ListPag(new BaseEntityFilter { Status = entity.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageSize = int.MaxValue });
                //ViewBag.gabinetes = listGabinetes.Select(x => new SelectListItem { Text = x.Name + " ("+x.ParlamentarNome+")", Value = x.Id.ToString() });

                ViewBag.produtoTipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(entity.ProdutoTipoId));


                //if (entity.Items.Count <=0)
                if (cleanModelState)
                    ModelState.Clear();

                await base.LoadViewBagEditMode(entity,cleanModelState);
            }
            else
                RedirectToAction("SelectTipos", nameof(SolicitacoesController));
        }

        [HttpGet]
        [ClaimRequirement(EUsuarioPerfil.Gestor, EUsuarioPerfil.Administrador, EUsuarioPerfil.Gabinete)]
        public async Task<IActionResult> NovaSolicitacao(NovaSolicitacaoModel model = null)
        {
            //ViewBag.toast = new ToastModel { Info = "TESTE" };


            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;


            model = model ?? new NovaSolicitacaoModel();
            model.Solicitacao = model.Solicitacao ?? new Solicitacao();
            model.Solicitacao.ProdutoTipoId = model.ProdutoTipoId;
           
            var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(model.ProdutoTipoId));
            var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));


            if (ptipo.HabilitaValoresDolar)
            {
                try
                {
                    var cotacao = await GetCotacaoDolar();

                    if (cotacao != null)
                    {
                        model.Solicitacao.CotacaoDolar = Math.Round(cotacao.Cotacao, 2);
                        model.Solicitacao.DataCotacao = cotacao.Data;
                    }
                }
                catch {
                    model.Solicitacao.CotacaoDolar = 1;
                    model.Solicitacao.DataCotacao = DateTime.Now;
                }
            }


           

            if (!profileTipo.PermiteSolicitar)
            {
                return Forbid();
            }

            foreach (var campo in ptipo.Campos.OrderBy(x => x.Ordem).ThenBy(x => x.Name))
            {
                model.Solicitacao.Campos.Add(new SolicitacaoCampos { Largura = campo.Largura, Name = campo.Name, Ordem = campo.Ordem, TipoCampo = campo.TipoCampo, Opcoes = campo.Opcoes, Grupo = campo.Grupo, Obrigatorio = campo.Obrigatorio, Posicao = campo.Posicao  });
            }


            await LoadViewBagEditMode(model.Solicitacao, true);

            


            return View(model);
        }

        [HttpPost]
        [ClaimRequirement(EUsuarioPerfil.Gestor, EUsuarioPerfil.Administrador, EUsuarioPerfil.Gabinete)]
        public async Task<IActionResult> SalvarNovaSolicitacao(NovaSolicitacaoModel model)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");

            Regex[] regex = new Regex[] 
            {   
                new Regex(@"^Solicitacao\.FileAnexo$"),
                new Regex(@"^Solicitacao\.Campos\[\d+\]\.Opcoes$"), 
                new Regex(@"^Solicitacao\.Campos\[\d+\]\.Grupo$"),
                new Regex(@"^Solicitacao\.Items\[\d+\]\.Produto.Unidade$"),
            };
            var valorCampo = new Regex(@"^Solicitacao\.Campos\[\d+\]\.Valor$");
            foreach (var entry in ModelState)
            {
                if (regex.Any(x=> x.IsMatch(entry.Key)))
                    ModelState[entry.Key].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            }

            for (int i =0; i< model.Solicitacao.Campos.Count; i++)
            {
                if (model.Solicitacao.Campos[i].Obrigatorio && String.IsNullOrWhiteSpace(model.Solicitacao.Campos[i].Valor))
                {
                    string key = "Solicitacao.Campos[" + i + "].Valor";

                    if (ModelState.ContainsKey(key))
                    {
                        ModelState[key].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
                        ModelState[key].Errors.Add("Campo " + model.Solicitacao.Campos[i].Name + " é obrigatório");
                    }

                   
                }
            }

            if (model.Solicitacao.Items.Count <= 0)
            {
                ModelState.AddModelError("Solicitacao.Items", "Adicione um item à solicitação");
            }


            if (ModelState.IsValid)
            {

                var user = await _userManager.GetUserAsync(User);
                var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                user.Role = role;
                var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(model.ProdutoTipoId));
                var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));
                if (!profileTipo.PermiteSolicitar)
                {
                    return Forbid();
                }

                if (!profileTipo.PermiteAprovar)
                {
                    model.Solicitacao.AprovacaoAutomatica = false;
                }

                try
                {
                    await _solicitacaoAppService.NovaSolicitacao(model, ptipo);
                    TempData.Add("toastSuccess", "Solicitação inserida com sucesso!");
                    return RedirectToAction(nameof(Index), new { ProdutoTipoId = model.ProdutoTipoId });
                }
                catch (Exception ex)
                {
                    await LoadViewBagEditMode(model.Solicitacao);
                    ViewBag.toast = new ToastModel { Error = "Erro: " + ex.Message };
                    return View(nameof(NovaSolicitacao), model);
                }
            }
            else
            {
                await LoadViewBagEditMode(model.Solicitacao);
                return View(nameof(NovaSolicitacao), model);
            }
        }

        protected override async Task LoadViewBagListMode(SolicitacaoFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;

            if (filter.ProdutoTipoId > 0)
                ViewBag.produtoTipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(filter.ProdutoTipoId));

            ViewBag.gabinetes = await _gabineteAppService.ListPag(new GabineteFilter { PageNumber = 1, PageSize = int.MaxValue, Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null });

            await base.LoadViewBagListMode(filter);
        }

        public override async Task<IActionResult> Save(Solicitacao entity)
        {

            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");

            Regex[] regex = new Regex[]
            {
                new Regex(@"^FileAnexo$"),
                new Regex(@"^Campos\[\d+\]\.Opcoes$"),
                new Regex(@"^Campos\[\d+\]\.Grupo$"),
                new Regex(@"^Items\[\d+\]\.Produto.Unidade$"),
                new Regex(@"^Anexos\[\d+\]\.Dados$"),
                new Regex(@"^Anexos\[\d+\]\.Name$")
            };
            //var valorCampo = new Regex(@"^Campos\[\d+\]\.Valor$");
            foreach (var entry in ModelState)
            {
                if (regex.Any(x => x.IsMatch(entry.Key)))
                    ModelState[entry.Key].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Skipped;
            }

            for (int i = 0; i < entity.Campos.Count; i++)
            {
                if (entity.Campos[i].Obrigatorio && String.IsNullOrWhiteSpace(entity.Campos[i].Valor))
                {
                    string key = "Campos[" + i + "].Valor";

                    if (ModelState.ContainsKey(key))
                    {
                        ModelState[key].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
                        ModelState[key].Errors.Add("Campo " + entity.Campos[i].Name + " é obrigatório");
                    }


                }
            }

            if (entity.Items.Count <= 0)
            {
                ModelState.AddModelError("Items", "Adicione um item à solicitação");
            }


            if (ModelState.IsValid)
            {

                try
                {
                    if (entity.Id <= 0)
                    {
                        var result = await _appServico.Cadastrar(entity);
                        TempData.Add("toastSuccess", "Registro inserido com sucesso!");
                    }
                    else
                    {
                        var user = await _userManager.GetUserAsync(User);
                        var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                        user.Role = role;
                        var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(entity.ProdutoTipoId));
                        var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));
                        if (profileTipo.TipoAlteracao == ETipoAleracoes.Nenhuma)
                        {
                            return Forbid();
                        }

                        if (!profileTipo.PermiteAprovar)
                        {
                            entity.AprovacaoAutomatica = false;
                        }



                        var result = await _appServico.Atualizar(entity);
                        TempData.Add("toastSuccess", "Registro atualizado com sucesso!");
                    }

                    return RedirectToAction(nameof(Index), new { ProdutoTipoId = entity.ProdutoTipoId });
                }
                catch (Exception ex)
                {
                    await LoadViewBagEditMode(entity);
                    ViewBag.toast = new ToastModel { Error = "Erro: " + ex.Message };
                    return View(nameof(Create), entity);
                }
            }
            await LoadViewBagEditMode(entity);
            return View(nameof(Create), entity);
        }

        public async Task<ActionResult> ModalAprovacao(SolicitacaoDto item)
        {
            var obj = await _appServico.Select(x => x.Id.Equals(item.Id));
            await LoadViewBagEditMode(obj);

            return View("Views/Solicitacoes/ModalAprovar.cshtml", obj);
        }

        public async Task<ActionResult> Recibo(int id)
        {
            var obj = await _appServico.Select(x => x.Id.Equals(id));
            await LoadViewBagEditMode(obj);
            return View(obj);
        }


        public async Task<ActionResult> ModalAprovar(Solicitacao model = null)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");
            var user = await _userManager.GetUserAsync(User);

            Solicitacao entity = await _solicitacaoAppService.Select(x => x.Id == model.Id);
            SolicitacaoDto solicitacaoDto = await _solicitacaoAppService.SelectDto(x => x.Id.Equals(entity.Id));

            if (solicitacaoDto != null)
            {
                var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                user.Role = role;
                var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(model.ProdutoTipoId));
                var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));
                if (!profileTipo.PermiteAprovar)
                {
                    return Forbid();
                }

                await _solicitacaoAppService.SetStatusSolicitacao(model.Id, ESolicitacaoStatus.Aprovado);
            }

            await LoadViewBagEditMode(entity);

            TempData.Add("toastSuccess", "Solicitação aprovada com sucesso!");

            return RedirectToAction(nameof(Index), new { ProdutoTipoId = entity.ProdutoTipoId });
        }

        public async Task<ActionResult> ModalRecusar(SolicitacaoDto model = null)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");


            var user = await _userManager.GetUserAsync(User);

            Solicitacao entity = await _solicitacaoAppService.Select(x => x.Id == model.Id);
            SolicitacaoDto solicitacaoDto = await _solicitacaoAppService.SelectDto(x => x.Id.Equals(entity.Id));

            if (solicitacaoDto != null)
            {
                var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                user.Role = role;
                var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(model.ProdutoTipoId));
                var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));
                if (!profileTipo.PermiteRecusar)
                {
                    return Forbid();
                }

                await _solicitacaoAppService.SetStatusSolicitacao(model.Id, ESolicitacaoStatus.Recusada);
            }

            await LoadViewBagEditMode(entity);

            TempData.Add("toastSuccess", "Solicitação recusada com sucesso!");

            return RedirectToAction(nameof(Index), new { ProdutoTipoId = entity.ProdutoTipoId });
        }

        public async Task<ActionResult> ModalCancelar(SolicitacaoDto model = null)
        {
            if (TempData.ContainsKey("toastSuccess"))
                TempData.Remove("toastSuccess");


            var user = await _userManager.GetUserAsync(User);

            Solicitacao entity = await _solicitacaoAppService.Select(x => x.Id == model.Id);
            SolicitacaoDto solicitacaoDto = await _solicitacaoAppService.SelectDto(x => x.Id.Equals(entity.Id));

            if (solicitacaoDto != null)
            {
                var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
                user.Role = role;
                var ptipo = await _produtoTipoAppService.SelectDto(x => x.Id.Equals(model.ProdutoTipoId));
                var profileTipo = ptipo.Usuarios.FirstOrDefault(x => x.Perfil.Equals(role));
                if (!profileTipo.PermiteCancelar)
                {
                    return Forbid();
                }

                await _solicitacaoAppService.SetStatusSolicitacao(model.Id, ESolicitacaoStatus.Cancelado);
            }

            await LoadViewBagEditMode(entity);

            TempData.Add("toastSuccess", "Solicitação cancelada com sucesso!");

            return RedirectToAction(nameof(Index), new { ProdutoTipoId = entity.ProdutoTipoId });
        }



        [HttpGet]
        public async Task<FileResult> AnexoDownload(int anexo)
        {
            var objAnexo = await  _solicitacaoAppService.GetAnexo(anexo);
            var contentType = GetContentType(objAnexo.Name);
            return File(objAnexo.Dados, contentType,  objAnexo.Name);
        }

        [HttpGet]
        public async Task<CotacaoDolarModel> GetCotacaoDolar()
        {
            var ultimasolicitacao = await _solicitacaoAppService.QueryDto(x => x.CotacaoDolar != 0).Where(x=>x.HabilitaValoresDolar).OrderByDescending(x=>x.DataSolicitacao).Take(1).FirstOrDefaultAsync();

            if (ultimasolicitacao != null)
            if (ultimasolicitacao.DataSolicitacao.Date == DateTime.Now.Date)
            {
                return new CotacaoDolarModel { Cotacao = ultimasolicitacao.CotacaoDolar.Value, Data = ultimasolicitacao.DataSolicitacao };
            }

            // 
            for (int i =0; i > -5; i--)
            {
                try
                {
                    DateTime dt = DateTime.Now.AddDays(i);
                    var url = "https://olinda.bcb.gov.br/olinda/servico/PTAX/versao/v1/odata/CotacaoMoedaDia(moeda='USD',dataCotacao='" + dt.ToString("MM-dd-yyyy") + "')";

                    using (HttpClientHandler handler = new HttpClientHandler())
                    {
                        // Use the default proxy settings configured in Windows
                        handler.Proxy = WebRequest.GetSystemWebProxy();
                        handler.UseProxy = true;

                        using (HttpClient client = new HttpClient(handler))
                        {
                            var json = await client.GetStringAsync(url);

                            if (json != null)
                            {
                                CotacaoBCB obj = JsonConvert.DeserializeObject<CotacaoBCB>(json);
                                var cotacao = obj.value.OrderByDescending(x => x.dataHoraCotacao).FirstOrDefault();
                                if (cotacao != null)
                                    return new CotacaoDolarModel { Cotacao = cotacao.cotacaoCompra, Data = cotacao.dataHoraCotacao };
                            }
                        }
                    }
                }
                catch  (Exception ex)
                {
                    _logger.LogError(ex, "Erro na consulta de cotação");
                }
            }

            if (ultimasolicitacao != null)
            {
               return new CotacaoDolarModel { Cotacao = ultimasolicitacao.CotacaoDolar.Value, Data = ultimasolicitacao.DataSolicitacao };
            }

            return null;
        }


        private string GetContentType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
