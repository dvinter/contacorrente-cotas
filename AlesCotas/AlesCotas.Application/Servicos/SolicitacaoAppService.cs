using AlesCotas.Application.Classes;
using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Application.Models;
using AlesCotas.Application.Servicos.Auth;
using AlesCotas.Application.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Servicos
{
    public class SolicitacaoAppService : AppServico<Solicitacao, SolicitacaoDto, SolicitacaoFilter>, ISolicitacaoAppService
    {
        private readonly ISolicitacaoRepository _solicitacaoRepository;
        private readonly IGabineteAppService _gabineteAppService;
        private readonly IMovimentoContaRepository _movimentoContaRepository;
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly IFluxoContaAppService _fluxoContaAppService;
        public SolicitacaoAppService(IMapper mapper,
            IMovimentoContaRepository movimentoContaRepository,
            ISolicitacaoRepository repository,
            ICurrentUserManager currentUserManager,
            IGabineteAppService gabineteAppService,
            IPublicacaoRepository publicacaoRepository,
             IFluxoContaAppService fluxoContaAppService,
            IWebHostEnvironment webHostEnvironment) : base(mapper, repository, currentUserManager)
        {
            _fluxoContaAppService = fluxoContaAppService;
            _publicacaoRepository = publicacaoRepository;
            _movimentoContaRepository = movimentoContaRepository;
            _solicitacaoRepository = repository;
            _gabineteAppService = gabineteAppService;
        }

        /// <summary>
        /// Retorna um DTO da solicitação, com dados relacionados como status atual e última data de modificação pelo log
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override IQueryable<SolicitacaoDto> QueryDto(Expression<Func<Solicitacao, bool>> predicate = null)
        {

            return this._repository
                .QueryAll(predicate)
                .SelectMany(b => _repository.Logs
                .Where(x => x.Entity
                            .Equals(_repository.GetTableName()) &&
                       x.RefId.Equals(b.Id))
                .OrderByDescending(l => l.Id).Take(1).DefaultIfEmpty(),
                (x, l) => new { x, l }).SelectMany(b => _solicitacaoRepository.QueryHistorico(null)
                                       .Where(x => x.SolicitacaoId.Equals(b.x.Id))
                                       .OrderByDescending(l => l.Sequencia).Take(1)
                                       .DefaultIfEmpty(),
                (x, h) =>
                new SolicitacaoDto
                {
                    Id = x.x.Id,
                    Numero = x.x.Numero,
                    DataSolicitacao = x.x.DataSolicitacao,
                    ProdutoTipoId = x.x.ProdutoTipoId,
                    ProdutoTipoDescricao = x.x.ProdutoTipo.Name,
                    HabilitaValoresDolar = x.x.ProdutoTipo.HabilitaValoresDolar,
                    UltimaAtualizacao = x.l.EventDate,
                    Observacoes = x.x.Observacoes,
                    GabineteId = x.x.GabineteId,
                    GabineteNome = x.x.Gabinete.Name,
                    QtdItens = x.x.Items.Count(),
                    TotalSolicitado = x.x.Total,
                    CotacaoDolar = x.x.CotacaoDolar,
                    DataCotacao = x.x.DataCotacao,
                    Publicado = _publicacaoRepository.QueryAll(null).Any(p => p.MesReferencia.Year == x.x.DataSolicitacao.Year && p.MesReferencia.Month == x.x.DataSolicitacao.Month && p.GabineteId.Equals(x.x.GabineteId)),
                    StatusAtual = new SolicitacaoHistoricoDto
                    {
                        DataRegistro = h.DataRegistro,
                        Sequencia = h.Sequencia,
                        SolicitacaoId = h.SolicitacaoId,
                        Status = h.Status,
                        UsuarioRegistroId = h.UsuarioRegistroId
                    }

                });
        }


        public override async Task<ListaPaginada<SolicitacaoDto>> ListPag(SolicitacaoFilter filter)
        {
            var query = QueryDto();


            var user = await _currentUserManager.GetCurrentUser();

            if (user.Role == EUsuarioPerfil.Gabinete)
            {
                query = query.Where(x => x.GabineteId.Equals(user.GabineteId));
            }

            if (filter.ProdutoTipoId > 0)
            {
                query = query.Where(x => x.ProdutoTipoId.Equals(filter.ProdutoTipoId));
            }


            if (filter.GabineteId > 0)
            {
                query = query.Where(x => x.GabineteId.Equals(filter.GabineteId));
            }

            if (!String.IsNullOrWhiteSpace(filter.FilterText))
            {
                query = query.Where(x => x.Observacoes.Contains(filter.FilterText) || x.GabineteNome.Contains(filter.FilterText) || x.Numero.StartsWith(filter.FilterText));
            }
            if (filter.SolicitacaoStatus != Domain.Filtros.Enum.EFiltroSolicitacaoStatus.Todos)
            {
                int status = (int)filter.SolicitacaoStatus;
                query = query.Where(x => (int)x.StatusAtual.Status == status);
            }
            if (filter.DataInicial.HasValue)
            {
                query = query.Where(x => x.DataSolicitacao.Date >= filter.DataInicial.Value.Date);
            }
            if (filter.DataFinal.HasValue)
            {
                query = query.Where(x => x.DataSolicitacao.Date <= filter.DataFinal.Value.Date);
            }

            query = query.OrderByDescending(x => x.DataSolicitacao).ThenByDescending(x => x.UltimaAtualizacao).ThenBy(x => x.GabineteNome);

            return await ToPagedList(query, filter.PageNumber, filter.PageSize);
        }


        public override async Task<Solicitacao> Select(Expression<Func<Solicitacao, bool>> predicate = null)
        {
            var entity = await _repository.QueryAll(predicate).Include(x=>x.Anexos).Include(x=>x.ProdutoTipo).ThenInclude(x=>x.Campos).Include(x=>x.Historico).ThenInclude(x=>x.UsuarioRegistro).FirstOrDefaultAsync();

            
            //Copia metadados do campo que vem do tipo de produto, para o campo da solicitação
            foreach (var campo in entity.Campos)
            {
                var tcampo = entity.ProdutoTipo.Campos.FirstOrDefault(x => x.Name.Equals(campo.Name));
                if (tcampo != null)
                {
                    campo.Opcoes = tcampo.Opcoes;
                    campo.Ordem = tcampo.Ordem;
                    campo.Largura = tcampo.Largura;
                    campo.Grupo = tcampo.Grupo;
                    campo.Obrigatorio = tcampo.Obrigatorio;
                    campo.TipoCampo = tcampo.TipoCampo;
                }
            }

            entity.Campos.Sort((a, b) => a.Ordem - b.Ordem);
            

            return entity; 
        }

        public override async Task<Solicitacao> Atualizar(Solicitacao entity)
        {
            var currentDto = await SelectDto(x => x.Id.Equals(entity.Id));
            if (currentDto.StatusAtual.Status != Domain.Enum.ESolicitacaoStatus.Pendente)
            {
                throw new Exception("Só é possível alterar uma solicitação pendente. Status atual: "+ currentDto.StatusAtual.Status.GetEnumDescription());
            }

            if (entity.FileAnexo != null)
            {
                foreach (var fileAnexo in entity.FileAnexo)
                {
                    if (fileAnexo.Length > 0)
                    {
                        using (MemoryStream fileStream = new MemoryStream())
                        {
                            await fileAnexo.CopyToAsync(fileStream);
                            entity.Anexos.Add(new SolicitacaoAnexos
                            {
                                Dados = fileStream.ToArray(),
                                Tamanho = entity.FileAnexo.Length,
                                Extensao = Path.GetExtension(fileAnexo.FileName),
                                Name = Path.GetFileName(fileAnexo.FileName),
                                DataArquivo = DateTime.Now
                            });
                        }
                    }
                }
            }


            return await base.Atualizar(entity);
        }

        public async Task<bool> NovaSolicitacao(NovaSolicitacaoModel model, ProdutoTipoDto produtoTipo)
        {
            var user = await _currentUserManager.GetCurrentUser();

            //Nível de isolamento serial pra não gerar o mesmo número de solicitação em duas instâncias diferentes
            using (var transaction = await _repository.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    DateTime inicio = produtoTipo.PermitirRecorrencia && model.RecorrenciaInicio.HasValue ?  model.RecorrenciaInicio.Value : DateTime.Now;
                    DateTime fim = produtoTipo.PermitirRecorrencia && model.RecorrenciaFim.HasValue ? model.RecorrenciaFim.Value : inicio;

                    for (DateTime dtSol = inicio; dtSol <= fim; dtSol = dtSol.AddMonths(1))
                    {
                        string suf = "/" + dtSol.ToString("yy");
                        var maxSol = await _solicitacaoRepository.QueryAll(x => x.Numero.EndsWith(suf))?.MaxAsync(x => x.Numero) ?? "0";
                        maxSol = maxSol.Contains("/") ? maxSol.Remove(maxSol.IndexOf("/")) : maxSol;
                        int seq = int.TryParse(maxSol, out int l) ? l : 0;

                        //garante que o número gerado não existe
                        do
                        {
                            seq++;
                        } while (await _solicitacaoRepository.QueryAll(x => x.Numero.Equals(seq.ToString("000000") + suf)).AnyAsync());

                        foreach (var gabId in model.Gabinetes)
                        {
                            var gabinete = await _gabineteAppService.Select(x => x.Id.Equals(gabId));

                            Solicitacao sol = model.Solicitacao.Clone();
                            sol.GabineteId = gabinete.Id;
                            sol.DataSolicitacao = dtSol;
                            sol.Numero = seq.ToString("000000") + suf;
                            seq++;

                            if (model.FileAnexo != null)
                            {
                                foreach (var fileAnexo in model.FileAnexo)
                                {

                                    if (fileAnexo.Length > 0)
                                    {
                                        using (MemoryStream fileStream = new MemoryStream())
                                        {
                                            await fileAnexo.CopyToAsync(fileStream);
                                            sol.Anexos.Add(new SolicitacaoAnexos
                                            {
                                                Dados = fileStream.ToArray(),
                                                Tamanho = model.FileAnexo.Length,
                                                Extensao = Path.GetExtension(fileAnexo.FileName),
                                                Name = Path.GetFileName(fileAnexo.FileName),
                                                DataArquivo = DateTime.Now
                                            });
                                        }
                                    }
                                }
                            }

                            sol.Historico.Add(new SolicitacaoHistorico { DataRegistro = DateTime.Now, Sequencia = 1, Status = ESolicitacaoStatus.Pendente, UsuarioRegistroId = user.Id });

                            if (user.Role == EUsuarioPerfil.Gabinete)
                            {
                                var extrato = await _fluxoContaAppService.SelectExtrato(new ExtratoFilter { GabineteId = gabinete.Id, MesInicio = new DateTime(dtSol.Year, 1, 1), MesFim = new DateTime(dtSol.Year, dtSol.Month, 1) });
                                if (extrato.SaldoDisponivel < (sol.Total))
                                {
                                    throw new Exception("Saldo do gabinete " + gabinete.Name + " é insuficiente para a solicitação. Disponível: " + extrato.SaldoDisponivel.ToString("N2"));
                                }
                            }

                            await _repository.Insert(sol, transaction);
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex.InnerException != null ? ex.InnerException : ex;
                }
            }
            return true;
        }


        public async Task<SolicitacaoAnexos> GetAnexo(int anexoId)
        {
            return await _solicitacaoRepository.QueryAnexos(x => x.Id.Equals(anexoId)).FirstOrDefaultAsync();
        }

        public async Task<bool> SetStatusSolicitacao(int id, ESolicitacaoStatus status)
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    var user = await _currentUserManager.GetCurrentUser();
                    var entity = await Select(x => x.Id.Equals(id));
                    var statusAtual = entity.Historico.LastOrDefault();

                    if (statusAtual.Status == ESolicitacaoStatus.Pendente)
                    {
                        int sequencia = statusAtual.Sequencia + 1;
                        _solicitacaoRepository.SetHistorico().Add(new SolicitacaoHistorico
                        {
                            DataRegistro = DateTime.Now,
                            Sequencia = sequencia,
                            Status = status,
                            UsuarioRegistroId = user.Id,
                            SolicitacaoId = entity.Id
                        });

                        if (status == ESolicitacaoStatus.Aprovado)
                        {
                            MovimentoConta movimentoConta = new MovimentoConta();
                            movimentoConta.Descricao = "Solicitação " + entity.ProdutoTipo.Name + ": " + entity.Numero;
                            movimentoConta.GabineteId = entity.GabineteId;
                            movimentoConta.Tipo = ETipoMovimento.Solicitacao;
                            movimentoConta.NaturezaMovimento = ENaturezaMovimento.Debito;
                            movimentoConta.Valor = entity.Total;
                            movimentoConta.DataMovimento = DateTime.Now;

                            await _movimentoContaRepository.Insert(movimentoConta, transaction);
                        }
                    }

                    await _repository.Commit();
                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<bool> ProcessarAprovacaoAutomatica()
        {
            using (var transaction = await _repository.BeginTransaction())
            {
                try
                {
                    Usuario user = await _currentUserManager.GetUser("system");
                    
                    if (user == null)
                    {
                        throw new Exception("Não foi possível encontrar um usuário de sistema para efetuar aprovações automáticas. Verifique o cadastro do usuário 'system'");
                    }

                    var solicitacoes = await QueryDto(x=>x.DataSolicitacao.Month == DateTime.Now.Month && x.DataSolicitacao.Year == DateTime.Now.Year && x.AprovacaoAutomatica).Where(x=>x.StatusAtual.Status == ESolicitacaoStatus.Pendente).ToListAsync();

                    if (solicitacoes != null && solicitacoes.Count > 0)
                    {
                        foreach (var sol in solicitacoes)
                        {
                            int sequencia = sol.StatusAtual.Sequencia + 1;
                            _solicitacaoRepository.SetHistorico().Add(new SolicitacaoHistorico
                            {
                                DataRegistro = DateTime.Now,
                                Sequencia = sequencia,
                                Status = ESolicitacaoStatus.Aprovado,
                                UsuarioRegistroId = user.Id,
                                SolicitacaoId = sol.Id
                            });

                            MovimentoConta movimentoConta = new MovimentoConta();
                            movimentoConta.Descricao = "Solicitação " + sol.ProdutoTipoDescricao + ": " + sol.Numero;
                            movimentoConta.GabineteId = sol.GabineteId;
                            movimentoConta.Tipo = ETipoMovimento.Solicitacao;
                            movimentoConta.NaturezaMovimento = ENaturezaMovimento.Debito;
                            movimentoConta.Valor = sol.TotalSolicitado;
                            movimentoConta.DataMovimento = DateTime.Now;

                            await _movimentoContaRepository.Insert(movimentoConta, transaction);
                        }
                        await _repository.Commit();
                        await transaction.CommitAsync();
                    }

                   

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}