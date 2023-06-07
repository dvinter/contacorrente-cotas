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
using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Auth;
using AlesCotas.Application.Servicos;
using System.Text;
using System.Globalization;
using System.Web;

namespace AlesCotas.Presentation.Controllers
{
    [ClaimRequirement(EUsuarioPerfil.Administrador, EUsuarioPerfil.Consulta)]
    public class PublicacoesController : BaseController<Publicacao,PublicacaoDto, PublicacaoFilter>
    {
        IGabineteAppService _gabineteAppService;
        ISolicitacaoAppService _solicitacaoAppService;
        public PublicacoesController(IPublicacaoAppService appService, IGabineteAppService gabineteAppService, UserManager<Usuario> userManager, ISolicitacaoAppService solicitacaoAppService) : base(appService, userManager)
        {
            _gabineteAppService = gabineteAppService;
            _solicitacaoAppService = solicitacaoAppService;
        }

        protected override async Task LoadViewBagEditMode(Publicacao entity = null, bool cleanModelState = false)
        {
            ViewBag.gabinetes = await _gabineteAppService.ListPag(new GabineteFilter {  Status = entity != null && entity.Id > 0 ? Domain.Filtros.Enum.EFiltroStatus.Todos : Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            await base.LoadViewBagEditMode(entity,cleanModelState);
        }

        protected override async Task LoadViewBagListMode(PublicacaoFilter filter)
        {
            ViewBag.gabinetes = await _gabineteAppService.ListPag(new GabineteFilter { Status = Domain.Filtros.Enum.EFiltroStatus.Ativo, PageNumber = 1, PageSize = int.MaxValue });
            await base.LoadViewBagListMode(filter);
        }


        public async Task<string> GetResumoGabinete(int gabinete, DateTime mes)
        {
            CultureInfo ptBr = new CultureInfo("pt-BR");
            var solicitacoes = await _solicitacaoAppService.QueryDto(x => x.GabineteId.Equals(gabinete) && x.DataSolicitacao.Year == mes.Year && x.DataSolicitacao.Month == mes.Month).Where(x=>x.StatusAtual.Status == ESolicitacaoStatus.Aprovado).ToListAsync();
            StringBuilder sb = new StringBuilder();
            var grupoTipos = solicitacoes.GroupBy(x => x.ProdutoTipoDescricao).Select(x => new { tipo= x.Key, total = x.Sum(y => y.TotalSolicitado) });
            foreach (var item in grupoTipos)
            {
                sb.AppendLine(item.tipo + ": " + item.total.ToString("C2",ptBr ) + ";");
            }
            sb.AppendLine("");
            sb.Append("Total: " + solicitacoes.Sum(x => x.TotalSolicitado).ToString("C2", ptBr));
            return sb.ToString();


        }
        public async Task<string> GetSolicitacoes(int gabinete, DateTime mes)
        {
            CultureInfo ptBr = new CultureInfo("pt-BR");
            var solicitacoes = await _solicitacaoAppService.QueryDto(x => x.GabineteId.Equals(gabinete) && x.DataSolicitacao.Year == mes.Year && x.DataSolicitacao.Month == mes.Month).Where(x => x.StatusAtual.Status == ESolicitacaoStatus.Pendente).ToListAsync();
            StringBuilder sb = new StringBuilder();
            var grupoTipos = solicitacoes.GroupBy(x => x.ProdutoTipoDescricao).Select(x => new { tipo = x.Key, total = x.Sum(y => y.TotalSolicitado) });
            if (solicitacoes.Any())
            {
                sb.AppendLine("Solicitações Pendentes em "+ mes.ToString("MMM/yyyy"));
                foreach (var item in grupoTipos)
                {
                    sb.AppendLine(item.tipo + ": " + item.total.ToString("C2", ptBr));
                }


            }

            return sb.ToString()?.Replace("\n","<br/>");
        }
       
    }
}
