using AlesCotas.Application.Classes;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class ProdutoTipoAppService : AppServico<ProdutoTipo, ProdutoTipoDto, ProdutoTipoFilter>, IProdutoTipoAppService
    {
        private readonly ISolicitacaoRepository _solicitacaoRepository;
        private readonly ISolicitacaoAppService _solicitacaoAppService;
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoTipoAppService(IMapper mapper, IProdutoTipoRepository repository, ISolicitacaoRepository solicitacaoRepository, IProdutoRepository produtoRepository, ICurrentUserManager currentUserManager, ISolicitacaoAppService solicitacaoAppService) : base(mapper, repository, currentUserManager)
        {
            _solicitacaoAppService = solicitacaoAppService;
            _solicitacaoRepository = solicitacaoRepository;
            _produtoRepository = produtoRepository;
            _solicitacaoAppService = solicitacaoAppService;
        }

        public override IQueryable<ProdutoTipoDto> QueryDto(Expression<Func<ProdutoTipo, bool>> predicate = null)
        {
            return this._repository.QueryAll(predicate).Include(x => x.Campos).Include(x => x.Usuarios)
                .SelectMany(b => _repository.Logs.Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) =>
                new ProdutoTipoDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Ativo = x.Ativo,
                    Icon = x.Icon,
                    UltimaAtualizacao = l.EventDate,
                    HabilitaAnexos = x.HabilitaAnexos,
                    HabilitaValoresDolar = x.HabilitaValoresDolar,
                    IncrementoQuantidade = x.IncrementoQuantidade,
                    ImprimirConferencia = x.ImprimirConferencia,
                    CancelamentoSolicitacaoAutomatico = x.CancelamentoSolicitacaoAutomatico,
                    PermitirRecorrencia = x.PermitirRecorrencia,
                    PermitirApenasUmItem = x.PermitirApenasUmItem,
                    CalcularDiarias = x.CalcularDiarias,
                    Gestores = x.Gestores.Select(u => new ProdutoTipoGestorDto { UsuarioId = u.UsuarioId, ProdutoTipoId = u.ProdutoTipoId }),
                    Campos = x.Campos.Select(c => new ProdutoTipoCamposDto { Id = c.Id, Largura = c.Largura, Name = c.Name, Ordem = c.Ordem, ProdutoTipoId = c.ProdutoTipoId, TipoCampo = c.TipoCampo, Opcoes = c.Opcoes, Grupo = c.Grupo, Obrigatorio = c.Obrigatorio, Posicao = c.Posicao }),
                    Usuarios = x.Usuarios.Select(c => new ProdutoTipoUsuarioPerfilDto { Perfil = c.Perfil, ProdutoTipoId = c.ProdutoTipoId, PermiteAprovar = c.PermiteAprovar, PermiteCancelar = c.PermiteCancelar, PermiteExcluir = c.PermiteExcluir, PermiteSolicitar = c.PermiteSolicitar, TipoAlteracao = c.TipoAlteracao, PermiteRecusar = c.PermiteRecusar, PermiteAlterarAprovado = c.PermiteAlterarAprovado })
                }) ;
        }

        public override async Task<ProdutoTipo> Select(Expression<Func<ProdutoTipo, bool>> predicate = null)
        {
            var entity = await base.Select(predicate);
            entity.GestoresId = entity.Gestores.Select(x => x.UsuarioId).ToList();
            return entity;
        }
        public override async Task<ListaPaginada<ProdutoTipoDto>> ListPag(ProdutoTipoFilter filter)
        {
            var query = QueryDto(filter.FilterExpression);

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Name.Contains(filter.FilterText));
            }
            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.Ativo == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Ativo));
            }

            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gestor)
            {
                query = query.Where(x => x.Gestores.Any(y => y.UsuarioId == user.Id));
            }


            query = query.OrderBy(x => x.Name);

            var lista = await ToPagedList(query, filter.PageNumber, filter.PageSize);
            var idsTipos = lista.Select(x => x.Id);

            var countProdutos = await _produtoRepository.QueryAll(x => idsTipos.Any(l => l.Equals(x.TipoId))).GroupBy(x => x.TipoId).Select(x=> new { x.Key, qtd= x.Count() }) .ToListAsync();

            var querySol = _solicitacaoAppService.QueryDto(x => idsTipos.Any(l => l.Equals(x.ProdutoTipoId))).Where(x => x.StatusAtual.Status.Equals(ESolicitacaoStatus.Pendente));

            if (user.GabineteId.HasValue && user.GabineteId > 0)
            {
                querySol = querySol.Where(x=>x.GabineteId.Equals(user.GabineteId)); 
            }

            var countSolicitacoes = await querySol.GroupBy(x => x.ProdutoTipoId).Select(x => new { x.Key, qtd = x.Count() }).ToListAsync();

            foreach (var item in lista)
            {
                item.QtdProdutos = countProdutos.Where(x => x.Key.Equals(item.Id)).Select(x => x.qtd).FirstOrDefault();
                item.QtdSolicitacoesPendentes = countSolicitacoes.Where(x => x.Key.Equals(item.Id)).Select(x => x.qtd).FirstOrDefault();
            }

            return lista;
        }

        public override Task<ProdutoTipo> Atualizar(ProdutoTipo entity)
        {
            foreach (var up in entity.Usuarios)
            {
                up.ProdutoTipo = entity;
                up.ProdutoTipoId = entity.Id;
            }

            foreach (var cp in entity.Campos)
            {
                //cp.ProdutoTipo = entity;
                cp.ProdutoTipoId = entity.Id;

            }

            return base.Atualizar(entity);
        }

    }
}
