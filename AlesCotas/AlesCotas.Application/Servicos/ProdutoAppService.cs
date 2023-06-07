using AlesCotas.Application.Classes;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class ProdutoAppService : AppServico<Produto, ProdutoDto, ProdutoFilter>, IProdutoAppService
    {
        public ProdutoAppService(IMapper mapper, IProdutoRepository repository, ICurrentUserManager currentUserManager) : base(mapper, repository, currentUserManager)
        {

        }


        public override IQueryable<ProdutoDto> QueryDto(Expression<Func<Produto, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate).SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new ProdutoDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Ativo = x.Ativo,
                    Descricao = x.Descricao,
                    TipoId = x.TipoId,
                    TipoNome = x.Tipo.Name,
                    Valor = x.Valor,
                    UltimaAtualizacao = l.EventDate,
                    Unidade = x.Unidade
                });
        }
        public override async Task<ListaPaginada<ProdutoDto>> ListPag(ProdutoFilter filter)
        {
            var query = QueryDto();

            if (filter.ProdutoTipoId >0)
            {
                query = query.Where(x => x.TipoId.Equals(filter.ProdutoTipoId));
            }

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Name.Contains(filter.FilterText));
            }
            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.Ativo == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }

        
    }
}