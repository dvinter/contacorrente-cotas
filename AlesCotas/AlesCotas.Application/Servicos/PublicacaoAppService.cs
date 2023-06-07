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


    public class PublicacaoAppService : AppServico<Publicacao, PublicacaoDto, PublicacaoFilter>, IPublicacaoAppService
    {
        private readonly ISolicitacaoAppService _solicitacaoAppService;
        public PublicacaoAppService(IMapper mapper, IPublicacaoRepository repository, ICurrentUserManager currentUserManager, ISolicitacaoAppService solicitacaoAppService) : base(mapper, repository, currentUserManager)
        {
            _solicitacaoAppService = solicitacaoAppService;
        }

        public override IQueryable<PublicacaoDto> QueryDto(Expression<Func<Publicacao, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate)
                .SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) =>
                new PublicacaoDto
                {
                    Id = x.Id,
                    Descricao = x.Descricao,
                    Ativo = x.Ativo,
                    UltimaAtualizacao = l.EventDate,
                    GabineteName = x.Gabinete.Name,
                    MesReferencia = x.MesReferencia,
                    GabineteId = x.GabineteId
                });
        }

        public override async Task<ListaPaginada<PublicacaoDto>> ListPag(PublicacaoFilter filter)
        {
            var dt = DateTime.Now;
            var query = QueryDto();

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Descricao.Contains(filter.FilterText));
            }

            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.Ativo == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }

            DateTime dtRef = new DateTime(filter.MesReferencia.Year, filter.MesReferencia.Month, 1);
            query = query.Where(x=>x.MesReferencia.Date == dtRef.Date);

            query = query.OrderBy(x => x.Descricao);

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }


        public override async Task<Publicacao> Cadastrar(Publicacao entity)
        {
            //TODO: validar se tem pendente
            if (await _solicitacaoAppService.QueryDto(x => x.GabineteId.Equals(entity.GabineteId)).Where(x=>x.StatusAtual.Status == Domain.Enum.ESolicitacaoStatus.Pendente).AnyAsync())
            {
                throw new Exception("Existe Solicitações pendentes para o gabinete.");
            }

            return await base.Cadastrar(entity);
        }


    }
}