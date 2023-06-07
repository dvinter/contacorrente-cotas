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
using System.Globalization;
using AlesCotas.Domain.Enum;
using AlesCotas.Infra.Data.Identity;

namespace AlesCotas.Application.Servicos
{


    public class CotaAppService : AppServico<Cota, CotaDto, BasePeriodoFilter>, ICotaAppService
    {
      
        ICotaRepository _cotaRepository;
        public CotaAppService(IMapper mapper, ICotaRepository repository,  ICurrentUserManager currentUserManager) : base(mapper, repository, currentUserManager)
        {
            _cotaRepository = repository;
        }
        public override IQueryable<CotaDto> QueryDto(Expression<Func<Cota, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate)
                .SelectMany(b => _cotaRepository.DbSetCotas.Where(f => f.Id > b.Id).OrderBy(l => l.Id).Take(1).DefaultIfEmpty(),
                (b, f) => new { b, f })
                .SelectMany(b => _repository.Logs.Include(x => x.Usuario).Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new CotaDto
                {
                    Id = x.b.Id,
                    Descricao = x.b.Descricao,
                    Lei = x.b.Lei,
                    DataInicio = x.b.DataInicio,
                    Valor = x.b.Valor,
                    DataFim = x.f != null ? x.f.DataInicio : null,
                    UltimaAtualizacao = l.EventDate,
                    UsuarioAtualizacao = l.Usuario.Nome,
                    MesLimiteAdiantamento = x.b.MesLimiteAdiantamento
                });
        }
        public override async Task<ListaPaginada<CotaDto>> ListPag(BasePeriodoFilter filter)
        {
            var query = QueryDto();

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Descricao.Contains(filter.FilterText) || x.Lei.Contains(filter.FilterText));
            }

            if (filter.DataInicial.HasValue)
            {
                query = query.Where(x => x.DataInicio.Date >= filter.DataInicial.Value.Date);
            }

            if (filter.DataFinal.HasValue)
            {
                query = query.Where(x => x.DataFim.HasValue && x.DataFim.Value.Date <= filter.DataFinal.Value.Date);
            }

            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => (x.DataInicio <= DateTime.Now && ((x.DataFim.HasValue && x.DataFim.Value > DateTime.Now) || !x.DataFim.HasValue)) == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }

            query = query.OrderByDescending(x => x.DataInicio <= DateTime.Now && ((x.DataFim.HasValue && x.DataFim.Value > DateTime.Now) || !x.DataFim.HasValue)).ThenBy(x => x.DataInicio);

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<CotaDto> SelectCotaAtual(DateTime dataReferencia)
        {
            var query = this._repository.QueryAll()
            .SelectMany(b => _cotaRepository.DbSetCotas.Where(f => f.Id > b.Id).OrderBy(l => l.Id).Take(1).DefaultIfEmpty(),
            (b, f) => new { b, f })
            .SelectMany(b => _repository.Logs.Include(x => x.Usuario).Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
            (x, l) => new CotaDto
            {
                Id = x.b.Id,
                Descricao = x.b.Descricao,
                Lei = x.b.Lei,
                DataInicio = x.b.DataInicio,
                Valor = x.b.Valor,
                DataFim = x.f != null ? x.f.DataInicio : null,
                UltimaAtualizacao = l.EventDate,
                UsuarioAtualizacao = l.Usuario.Nome,
                MesLimiteAdiantamento = x.b.MesLimiteAdiantamento
            });

            return await query.FirstOrDefaultAsync(x=> (x.DataInicio <= dataReferencia && ((x.DataFim.HasValue && x.DataFim.Value > dataReferencia) || !x.DataFim.HasValue)));
           
        }

        public override async Task<Cota> Cadastrar(Cota entity)
        {
            entity.DataInicio = entity.DataInicio.Date;
            if (await _repository.QueryAll(x => x.DataInicio > entity.DataInicio).AnyAsync())
            {
                throw new Exception("Data Inicial não pode ser menor que uma data já cadastrada");
            }
            return await base.Cadastrar(entity);
        }

        public override async Task<Cota> Atualizar(Cota entity)
        {
            entity.DataInicio = entity.DataInicio.Date;
            if (await _repository.QueryAll(x => x.DataInicio > entity.DataInicio && x.Id < entity.Id).AnyAsync())
            {
                throw new Exception("Data Inicial não pode ser menor que uma data já cadastrada");
            }
            return await base.Atualizar(entity);
        }

        public async Task<DateTime> UltimoProcessamentoCotas()
        {
            return await _repository.Logs.Where(x => x.EventType.Equals(ETipoEventoLog.ProcessamentoCotas)).OrderByDescending(x=>x.Id).Select(x=> new DateTime(x.EventDate.Year, x.EventDate.Month,1) ).FirstOrDefaultAsync();
        }

        public async Task<int> ProcessarCotas()
        {
            var cota = await ListPag(new BasePeriodoFilter { Status = Domain.Filtros.Enum.EFiltroStatus.Ativo });
            if (cota.Any())
            {
                return await _cotaRepository.ProcessarCotas(cota.Select(x => x.Id).FirstOrDefault());
            }
            return -1;
        }
    }
}