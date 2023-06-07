using AlesCotas.Application.Dto;
using AlesCotas.Application.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Filtros;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlesCotas.Application.Interfaces.Servicos;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories;
using AlesCotas.Application.Filter;

namespace AlesCotas.Application.Servicos
{


    public class GabineteAppService : AppServico<Gabinete, GabineteDto, GabineteFilter>, IGabineteAppService
    {

        private readonly IFluxoContaRepository _fluxoContaRepository;
      
        public GabineteAppService(IMapper mapper, IGabineteRepository repository, ICurrentUserManager currentUserManager, IFluxoContaRepository fluxoContaRepository) : base(mapper, repository,currentUserManager)
        {
            _fluxoContaRepository = fluxoContaRepository;
        }

        public override IQueryable<GabineteDto> QueryDto(Expression<Func<Gabinete, bool>> predicate = null)
        {
            var dt = DateTime.Now;
            return 
            this._repository.QueryAll(predicate)
                .SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new { x, l })
                .SelectMany(b => _fluxoContaRepository.QueryExtratoDataRegistro().Where(x => x.GabineteId.Equals(b.x.Id) && x.Ano.Equals(dt.Year) && x.Mes <= dt.Month).OrderByDescending(e => e.RowNumber).Take(1).DefaultIfEmpty(),
                (x, e) => new { x, e })
                .SelectMany(b => _fluxoContaRepository.QueryTotalSolicitacoes().Where(x=>x.GabineteId.Equals(b.x.x.Id) && x.Status == Domain.Enum.ESolicitacaoStatus.Pendente && x.MesReferencia.Year == dt.Year && x.MesReferencia.Month == dt.Month).DefaultIfEmpty(),
                (x, s) => 
                new GabineteDto
                {
                    Id = x.x.x.Id,
                    Name = x.x.x.Name,
                    Ativo = x.x.x.Ativo,
                    UltimaAtualizacao = x.x.l.EventDate,
                    SaldoAtualx = x.e.SaldoAtual,
                    SaldoReservado = s.TotalSolicitacao
                });
        }

        public override async Task<ListaPaginada<GabineteDto>> ListPag(GabineteFilter filter)
        {
            var query = QueryDto();

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Name.Contains(filter.FilterText));
            }
            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.Ativo == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }

            if (filter.GabineteId.HasValue)
            {
                query = query.Where(x => x.Id.Equals(filter.GabineteId.Value));
            }

            query = query.OrderBy(x => x.Name);

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }

    }
}