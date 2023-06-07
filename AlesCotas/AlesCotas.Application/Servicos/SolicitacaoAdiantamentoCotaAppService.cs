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
using AlesCotas.Application.Filter;

namespace AlesCotas.Application.Servicos
{


    public class SolicitacaoAdiantamentoCotaAppService : AppServico<SolicitacaoAdiantamentoCota, SolicitacaoAdiantamentoCotaDto, SolicitacaoAdiantamentoCotaFilter>, ISolicitacaoAdiantamentoCotaAppService
    {

        ISolicitacaoAdiantamentoCotaRepository _solAdcotaRepository;
        private readonly IMovimentoContaAppService _movimentoContaAppService;
        private readonly ICotaAppService _cotaAppService;
            public SolicitacaoAdiantamentoCotaAppService(IMapper mapper,
                ISolicitacaoAdiantamentoCotaRepository repository,  
                ICurrentUserManager currentUserManager, 
                IMovimentoContaAppService       movimentoContaAppService,
                ICotaAppService cotaAppService
                ) : base(mapper, repository, currentUserManager)
        {
            _solAdcotaRepository = repository;
            _movimentoContaAppService = movimentoContaAppService;
            _cotaAppService = cotaAppService;
        }


        public override IQueryable<SolicitacaoAdiantamentoCotaDto> QueryDto(Expression<Func<SolicitacaoAdiantamentoCota, bool>> predicate = null)
        {
           return this._repository.QueryAll(predicate)
                .SelectMany(b => _repository.Logs.Include(x => x.Usuario).Where(x => x.Entity.Equals(_repository.GetTableName()) && x.RefId.Equals(b.Id)).OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new SolicitacaoAdiantamentoCotaDto
                {
                    Id = x.Id,
                    Descricao = x.Descricao,
                    DataResposta = x.DataResposta,
                    DataSolicitacao = x.DataSolicitacao,
                    GabineteId = x.GabineteId,
                    GabineteNome = x.Gabinete.Name,
                    Mensagem = x.Mensagem,
                    MesReferente = x.MesReferente,
                    UsuarioRespostaId = x.UsuarioRespostaId,
                    UsuarioRespostaNome = x.UsuarioResposta.Nome,
                    UltimaAtualizacao = l.EventDate,
                });
        }
        public override async Task<ListaPaginada<SolicitacaoAdiantamentoCotaDto>> ListPag(SolicitacaoAdiantamentoCotaFilter filter)
        {
            var query = QueryDto();

            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                query = query.Where(x => x.GabineteId.Equals(user.GabineteId));
            }
            else
            {
                if (filter.GabineteId.HasValue)
                    query = query.Where(x => x.GabineteId.Equals(filter.GabineteId.Value));
            }

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Descricao.Contains(filter.FilterText) || x.GabineteNome.Contains(filter.FilterText));
            }

            if (filter.DataInicial.HasValue)
            {
                query = query.Where(x => x.DataSolicitacao.Date >= filter.DataInicial.Value.Date);
            }

            if (filter.DataFinal.HasValue)
            {
                query = query.Where(x => x.DataSolicitacao.Date <= filter.DataFinal.Value.Date);
            }

            if (filter.Status != Domain.Filtros.Enum.EFiltroStatus.Todos)
            {
                query = query.Where(x => x.DataResposta.HasValue == (filter.Status == Domain.Filtros.Enum.EFiltroStatus.Inativo));
            }

            query = query.OrderByDescending(x => x.DataSolicitacao);

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<List<DateTime>> MesesJaRecebidos(int gabineteId)
        {
            var datas = await _movimentoContaAppService.QueryDto(x => x.Tipo.Equals(ETipoMovimento.Cota)
            && x.DataReferenciaCota.HasValue
            && x.GabineteId.Equals(gabineteId)).Select(x => x.DataReferenciaCota.Value).Distinct().ToListAsync();

            return datas;
        }

        public override async  Task<SolicitacaoAdiantamentoCota> Cadastrar(SolicitacaoAdiantamentoCota entity)
        {
            var qtd = await _movimentoContaAppService
            .Count(x => x.Tipo.Equals(ETipoMovimento.Cota)
            && x.DataReferenciaCota.HasValue
            && x.DataReferenciaCota.Value.Date.Equals(entity.MesReferente.Date)
            && x.GabineteId.Equals(entity.GabineteId)
            );

            if (qtd > 0)
            {
                throw new Exception("Cota do mês solicitado já foi recebida / processada");
            }

            

            entity.DataSolicitacao = DateTime.Now;

            return await base.Cadastrar(entity);

            //entity.UsuarioRespostaId = user.Id;
            //entity.DataResposta = DateTime.Now;
            //entity.Mensagem = "Solicitação aprovada por " + user.Nome;

            
            //var entry = await _repository.Insert(entity);

            //var cota = await _cotaAppService.SelectCotaAtual(entity.MesReferente);
            //var movimento = await _movimentoContaAppService.Cadastrar(new MovimentoConta
            //{
            //    DataMovimento = DateTime.Now.Date,
            //    DataReferenciaCota = entity.MesReferente,
            //    Descricao = "Adiantamento de cota - " + entity.MesReferente.ToString("MMM/yyyy"),
            //    GabineteId = entity.GabineteId,
            //    NaturezaMovimento = ENaturezaMovimento.Credito,
            //    Tipo = ETipoMovimento.Cota,
            //    Valor = cota.Valor
            //});

            //await _repository.Commit();
            //return entry;            
        }

        public async Task<bool> Recusar(int id)
        {
            var entity = await _repository.QueryAll(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (entity.DataResposta.HasValue)
            {
                throw new Exception("Solicitação já respondida");
            }

            var user = await _currentUserManager.GetCurrentUser();
            var cota = await _cotaAppService.SelectCotaAtual(entity.MesReferente);
            entity.UsuarioRespostaId = user.Id;
            entity.DataResposta = DateTime.Now;
            entity.Mensagem = "Solicitação recusada por " + user.Nome;

            await _repository.Update(entity);
            await _repository.Commit();
            return true;
        }
        public async Task<bool> Aprovar(int id)
        {
            var entity = await _repository.QueryAll(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (entity.DataResposta.HasValue)
            {
                throw new Exception("Solicitação já respondida");
            }

            var qtd = await _movimentoContaAppService
            .Count(x => x.Tipo.Equals(ETipoMovimento.Cota)
            && x.DataReferenciaCota.HasValue
            && x.DataReferenciaCota.Value.Date.Equals(entity.MesReferente.Date)
            && x.GabineteId.Equals(entity.GabineteId)
            );

            if (qtd > 0)
            {
                throw new Exception("Cota do mês solicitado já foi recebida / processada");
            }
            
            var user = await _currentUserManager.GetCurrentUser();
            var cota = await _cotaAppService.SelectCotaAtual(entity.MesReferente);
            entity.UsuarioRespostaId = user.Id;
            entity.DataResposta = DateTime.Now;
            entity.Mensagem = "Solicitação aprovada por " + user.Nome;

            var movimento =  await _movimentoContaAppService.Cadastrar(new MovimentoConta
            {
                DataMovimento = DateTime.Now.Date,
                DataReferenciaCota = entity.MesReferente,
                Descricao = "Adiantamento de cota - "+ entity.MesReferente.ToString("MMM/yyyy"),
                GabineteId = entity.GabineteId,
                 NaturezaMovimento = ENaturezaMovimento.Credito,
                  Tipo = ETipoMovimento.Cota,
                   Valor = cota.Valor                    
            });

            await _repository.Update(entity);
            await _repository.Commit();
            return true;

        }

       
    }
}