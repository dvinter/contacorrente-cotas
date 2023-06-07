using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Model;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Domain.Model;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories;
using AlesCotas.Infra.Data.SqlQuery;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class DashBoardAppService : IDashBoardAppService
    {
        private readonly
            IGabineteRepository _gabineteRepository;
        ICurrentUserManager _currentUserManager;
        private readonly IFluxoContaAppService _fluxoContaAppService;
        private readonly ISolicitacaoAppService _solicitacaoAppService;
        private readonly IFluxoContaRepository _fluxoContaRepository;
        private readonly IProdutoTipoRepository _produtoTipoRepository;
        public DashBoardAppService(
            IGabineteRepository gabineteRepository,
            IFluxoContaRepository fluxoContaRepository,
        IFluxoContaAppService fluxoContaAppService,
            ISolicitacaoAppService solicitacaoAppService,
            IProdutoTipoRepository produtoTipoRepository,
            ICurrentUserManager currentUserManager)
        {
            _produtoTipoRepository = produtoTipoRepository;
            _fluxoContaRepository = fluxoContaRepository;
            _solicitacaoAppService = solicitacaoAppService;
            _currentUserManager = currentUserManager;
            _fluxoContaAppService = fluxoContaAppService;
            _gabineteRepository = gabineteRepository;
        }


        public async Task<IEnumerable<GabineteDto>> GetGabinetes()
        {
            var query = _gabineteRepository.QueryAll(x => x.Ativo);
            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                query = query.Where(x => x.Id.Equals(user.GabineteId));
            }

            query = query.OrderBy(x => x.Name);

            return await query.Select(x => new GabineteDto { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        public async Task<IEnumerable<ProdutoTipoDto>> GetProdutoTipos()
        {
            var query = _produtoTipoRepository.QueryAll(x => x.Ativo);
            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gestor)
            {
                query = query.Where(x =>  x.Gestores.Any(y=> y.UsuarioId == user.Id)); //!x.GestorId.HasValue ||
            }

            query = query.OrderBy(x => x.Name);

            return await query.Select(x => new ProdutoTipoDto { Id = x.Id, Name = x.Name }).ToListAsync();
        }

        public async Task<DashBoardViewModel> GetDashBoard(DashBoardFilter filtro)
        {
            var user = await _currentUserManager.GetCurrentUser();
           
            DashBoardViewModel obj = new DashBoardViewModel();
            var querySol = _solicitacaoAppService.QueryDto(x => x.DataSolicitacao.Date >= filtro.DataInicio.Date && x.DataSolicitacao.Date <= filtro.DataFim.AddMonths(1).AddDays(-1));
            if (filtro.ProdutoTipoId != 0)
            {
                querySol = querySol.Where(x => x.ProdutoTipoId.Equals(filtro.ProdutoTipoId));
            }

            if (user.Role == EUsuarioPerfil.Gestor)
            {
                var tiposGestor = await _produtoTipoRepository.QueryAll(x => x.Ativo)
               .Where(x => x.Gestores.Any(y => y.UsuarioId.Equals(user.Id))).Select(x => x.Id).ToListAsync(); //!x.GestorId.HasValue ||
                querySol = querySol.Where(x => tiposGestor.Contains(x.ProdutoTipoId));
            }

           
            var queryFluxo = _fluxoContaRepository.QueryExtratoDataMovimento().Where(x => 
            x.DataMovimento.Year >= filtro.DataInicio.Year 
            && x.DataMovimento.Year <= filtro.DataFim.Year 
            && x.DataMovimento.Month >= filtro.DataInicio.Month 
            && x.DataMovimento.Month <= filtro.DataFim.Month
            && x.SequenciaMovimento == 1 && !x.Estorno);

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                querySol = querySol.Where(x => x.GabineteId.Equals(user.GabineteId));
                queryFluxo = queryFluxo.Where(x => x.GabineteId.Equals(user.GabineteId));
            }
            else
            {
                querySol = querySol.Where(x => (x.GabineteId.Equals(filtro.GabineteId) || filtro.GabineteId == 0));
                queryFluxo = queryFluxo.Where(x => (x.GabineteId.Equals(filtro.GabineteId) || filtro.GabineteId == 0));
            }

            var solicitacoes  = await querySol.ToListAsync();
            var fluxo = await queryFluxo.ToListAsync();

            int qtdUltimas = user.Role == EUsuarioPerfil.Gestor ? 20 : 10;

            obj.UltimasSolicitacoes = solicitacoes.OrderByDescending(x => x.DataSolicitacao).Take(qtdUltimas).ToList();
            obj.TotalSolicitacoesStatus = solicitacoes.Where(x => x.StatusAtual.Status != ESolicitacaoStatus.Cancelado).GroupBy(x => x.StatusAtual.Status).Select(x => new DashSolicitacoesModel { Status = x.Key, Total = x.Sum(y => y.TotalSolicitado) }).ToList();
            obj.TotalPendente = solicitacoes.Where(x => x.StatusAtual.Status == ESolicitacaoStatus.Pendente 
                //&& x.DataSolicitacao.Date >= filtro.DataInicio.Date
                //&& x.DataSolicitacao.Date <= filtro.DataFim.AddMonths(1).AddDays(-1)
            ).Sum(x => x.TotalSolicitado);
            obj.TotalCreditos = fluxo.Where(x => x.Valor > 0).Sum(x => x.Valor);
            obj.TotalDebitos = fluxo.Where(x => x.Valor < 0).Sum(x => x.Valor);
            List<dynamic> arrayChartData = new List<dynamic>();
            arrayChartData.Add(new object[] { "Mês","Solicitações", "Crédito", "Débito", "Pendente", "Saldo" });

            int idx = 1;
            for (DateTime dt = filtro.DataInicio; dt <= filtro.DataFim.AddMonths(1).AddDays(-1); dt = dt.AddMonths(1))
            {
                var fluxomes = fluxo.Where(x => x.DataMovimento.Year == dt.Year && x.DataMovimento.Month == dt.Month);
                decimal pendenteMes = Math.Round(solicitacoes.Where(x => x.DataSolicitacao.Year == dt.Year && x.DataSolicitacao.Month == dt.Month && x.StatusAtual.Status == ESolicitacaoStatus.Pendente).Sum(x => x.TotalSolicitado),2);
                decimal totalSolicitacoes = solicitacoes.Where(x => x.DataSolicitacao.Year == dt.Year && x.DataSolicitacao.Month == dt.Month).Sum(x => x.TotalSolicitado);
                decimal creditoMes = fluxomes.Where(x => x.Valor > 0 /*&& x.Tipo != ETipoMovimento.TransferenciaSaldo*/).Sum(x => x.Valor);
                decimal debitoMes = fluxomes.Where(x => x.Valor < 0 /*&& x.Tipo != ETipoMovimento.TransferenciaSaldo*/).Sum(x => x.Valor);
                decimal saldoMes = creditoMes + debitoMes - pendenteMes;
                arrayChartData.Add(new object[] 
                { 
                    dt.ToString("MMM/yyyy"), 
                    totalSolicitacoes,
                    creditoMes,
                    debitoMes*-1,
                    pendenteMes, 
                    saldoMes
                });

                idx++;
            }

            obj.ChartData = arrayChartData;

            return obj;


        }
    }
}
