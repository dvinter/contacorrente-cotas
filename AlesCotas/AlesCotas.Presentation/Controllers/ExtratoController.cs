using AlesCotas.Application.Filter;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Servicos;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;

namespace AlesCotas.Presentation.Controllers
{
    [Authorize]
    public class ExtratoController : Controller
    {
        private readonly IFluxoContaAppService _fluxoContaAppService;
        private readonly IGabineteAppService _gabineteAppService;
        protected readonly UserManager<Usuario> _userManager;

        public ExtratoController(IFluxoContaAppService appService, IGabineteAppService gabineteAppService, UserManager<Usuario> userManager)
        {
            _gabineteAppService = gabineteAppService;
            _fluxoContaAppService = appService;
            _userManager = userManager; 

        }
        public virtual void VerificaTempDataToast()
        {
            if (TempData.ContainsKey("toastSuccess"))
                ViewBag.toast = new ToastModel { Success = TempData["toastSuccess"].ToString() };
        }
        public async Task<IActionResult> Index(ExtratoFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            ViewBag.user = user;

            if (user.GabineteId.HasValue)
            {
                filter.GabineteId = user.GabineteId.Value;
            }

            var listGabinetes = (await _gabineteAppService.ListPag(new Application.Filter.GabineteFilter { PageSize = int.MaxValue, Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, GabineteId = user.Role == EUsuarioPerfil.Gabinete ? user.GabineteId.Value : null })).ToList();
           
            ViewBag.gabinetes = listGabinetes.Where(x=> user.GabineteId.HasValue ?  x.Id.Equals(user.GabineteId.Value) : true).OrderBy(x=>x.Name).ToList();
            ViewBag.filtro = filter;
            var lista = await  _fluxoContaAppService.SelectExtrato(filter);

            VerificaTempDataToast();

            return View(lista);
        }

        [HttpGet]
        public async Task<FileResult> ExportarExtrato(ExtratoFilter filter)
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>().FirstOrDefault();
            user.Role = role;
            ViewBag.user = user;

            if (user.GabineteId.HasValue)
            {
                filter.GabineteId = user.GabineteId.Value;
            }
            var lista = await _fluxoContaAppService.SelectExtrato(filter);
            byte[] dados = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                using (TextWriter tw = new StreamWriter(ms, Encoding.UTF8))
                {
                    tw.WriteLine("sequencia;movimento_id;data_movimento;data_registro;usuario_registro;gabinete_id;gabinete_nome;descricao;tipo;tipo_descricao;valor;saldo_anterior;saldo_atual;sequencia_movimento;estorno;ano;mes");

                    foreach (var linha in lista.Movimentos)
                    {
                        tw.WriteLine(linha.Sequencia + ";" + linha.MovimentoId + ";" + linha.DataMovimento + ";" + linha.DataRegistro + ";" + linha.UsuarioRegistro + ";" + linha.GabineteId + ";" + linha.GabineteName + ";" + linha.Descricao + ";" + linha.Tipo + ";" + linha.TipoDescricao + ";" + linha.Valor + ";" + linha.SaldoAnterior + ";" + linha.SaldoAtual + ";" + linha.SequenciaMovimento + ";" + linha.Estorno + ";" + linha.Ano + ";" + linha.Mes);
                    }
                }
                dados = ms.ToArray();
            }

            string filename = "Extrato_" + filter.GabineteId + "_" + filter.MesInicio.ToString("yyyy/MMMM") + ".csv";
            return File(dados, "text/csv",filename );
        }
    }
}
