using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Domain.Model;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
 

    public class FluxoContaAppService : IFluxoContaAppService
    {
        private readonly IFluxoContaRepository _fluxoContaRepository;

        ICurrentUserManager _currentUserManager;
        public FluxoContaAppService(IFluxoContaRepository fluxoContaRepository,
            ICurrentUserManager currentUserManager
            )
        {
            _currentUserManager = currentUserManager;
            _fluxoContaRepository = fluxoContaRepository;
        }

        public async Task<int> ProcessarFila()
        {
            return await _fluxoContaRepository.ProcessarFila();
        }


        public async Task<ExtratoViewModel> SelectExtrato(ExtratoFilter filter)
        {
            IQueryable<ExtratoModel> query;
            if (filter.TipoData == Domain.Filtros.Enum.EFiltroTipoData.DataRegistro)
            {
                query = _fluxoContaRepository.QueryExtratoDataRegistro().Where(x => x.GabineteId == filter.GabineteId &&
                x.DataRegistro.Year >= filter.MesInicio.Year &&
                x.DataRegistro.Month >= filter.MesInicio.Month &&
                x.DataRegistro.Year <= filter.MesFim.Year &&
                x.DataRegistro.Month <= filter.MesFim.Month
                );

            }
            else
            {
                query = _fluxoContaRepository.QueryExtratoDataMovimento().Where(x => x.GabineteId == filter.GabineteId &&
                x.DataMovimento.Year >= filter.MesInicio.Year &&
                x.DataMovimento.Month >= filter.MesInicio.Month &&
                x.DataMovimento.Year <= filter.MesFim.Year &&
                x.DataMovimento.Month <= filter.MesFim.Month
                );
            }
            
            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                query = query.Where(x => x.GabineteId.Equals(user.GabineteId));
            }

            query = query.OrderBy(x => x.RowNumber);

            var sql = query.ToQueryString();
            var viewModel = new ExtratoViewModel();
            viewModel.Movimentos = await query.ToListAsync();
            viewModel.SaldoAtual = viewModel.Movimentos.LastOrDefault()?.SaldoAtual ?? 0;
            viewModel.SaldoReservado = 0;
            if (filter.GabineteId > 0)
            {
                var querySol = _fluxoContaRepository.QueryTotalSolicitacoes().Where(x => x.GabineteId.Equals(filter.GabineteId) 
                && x.MesReferencia.Year >= filter.MesInicio.Year 
                && x.MesReferencia.Month >= filter.MesInicio.Month
                && x.MesReferencia.Year <= filter.MesFim.Year
                && x.MesReferencia.Month <= filter.MesFim.Month);

                var solicitacoes = await querySol.ToListAsync();
                viewModel.SaldoReservado = solicitacoes?.FirstOrDefault(x => x.Status == ESolicitacaoStatus.Pendente)?.TotalSolicitacao ?? 0;
            }
            return viewModel;
        }
       

    }
}
