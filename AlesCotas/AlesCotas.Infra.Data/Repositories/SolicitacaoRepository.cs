using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Arm;

namespace AlesCotas.Infra.Data.Repositories
{
    public  class SolicitacaoRepository : Repository<Solicitacao>, ISolicitacaoRepository
    {
        public SolicitacaoRepository(ApplicationDbContext context, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {
        }

       

        public override IQueryable<Solicitacao> QueryAll(Expression<Func<Solicitacao, bool>> predicate = null)
        {
            return base.QueryAll(predicate).Include(x=>x.Gabinete).Include(x=>x.Items).ThenInclude(x=>x.Produto).Include(x=>x.Campos).Include(x=>x.Historico);
        }

        public IQueryable<SolicitacaoHistorico> QueryHistorico(Expression<Func<SolicitacaoHistorico, bool>> predicate = null)
        {
            var query = this.Context.SolicitacaoHistorico.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public IQueryable<SolicitacaoItem> QueryItens(Expression<Func<SolicitacaoItem, bool>> predicate = null)
        {
            var query = this.Context.SolicitacaoItem.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }


        public override Task<Solicitacao> Insert(Solicitacao entity, IDbContextTransaction? transaction=null)
        {
            int sequencia = 0;
            foreach (var item in entity.Items)
            {
                item.Produto = null;
                item.Sequencia = ++sequencia;
            }
            return base.Insert(entity, transaction);
        }

        public override async Task<Solicitacao> Update(Solicitacao entity, IDbContextTransaction? transacao = null)
        {
            var currentdb = await this.Context.Solicitacao.Include(x=>x.Campos).Include(x=>x.Items).Include(x=>x.Historico).Include(x=>x.Anexos).FirstOrDefaultAsync(x=>x.Id.Equals(entity.Id));

            if (currentdb == null)
                throw new Exception("Registro não encontrado");

            var currentStatus = currentdb.Historico.Last();

            if (currentStatus.Status != Domain.Enum.ESolicitacaoStatus.Pendente)
            {
                throw new Exception("Só é possível alterar uma solicitação pendente.");
            }
            var user = await this._currentUserManager.GetCurrentUser();

            var tipo = await Context.ProdutoTipo.Include(x => x.Usuarios).FirstAsync(x => x.Id.Equals(entity.ProdutoTipoId));
            var profile = tipo.Usuarios.First(x => x.Perfil.Equals(user.Role));

            if (profile.TipoAlteracao == ETipoAleracoes.AumentarQuantidades)
            {
                if (entity.Items.Sum(x=>x.QuantidadeSolicitada) < currentdb.Items.Sum(x=>x.QuantidadeSolicitada))
                {
                    throw new Exception("Usuário sem permissão para diminuir quantidades solicitadas.");
                }
            }
            if (profile.TipoAlteracao == ETipoAleracoes.DiminuirQuantidades)
            {
                if (entity.Items.Sum(x => x.QuantidadeSolicitada) > currentdb.Items.Sum(x => x.QuantidadeSolicitada))
                {
                    throw new Exception("Usuário sem permissão para aumentar quantidades solicitadas.");
                }
            }

            currentdb.Historico.Add(new SolicitacaoHistorico
            {
                DataRegistro = DateTime.Now,
                Sequencia = currentStatus.Sequencia + 1,
                Status = currentStatus.Status,
                UsuarioRegistroId = user.Id,
                Justificativa = entity.NovaJustificativa
            });


            currentdb.Items.Clear();

            int sequencia = 0;
            foreach (var item in entity.Items)
            {
                item.Produto = null;
                item.Sequencia = ++sequencia;

                currentdb.Items.Add(item);

            }

            currentdb.Campos.Clear();

            foreach (var campo in entity.Campos)
            {
                currentdb.Campos.Add(campo);
            }

            for (int i = 0; i < currentdb.Anexos.Count; i++)
            {
                if (entity.Anexos.Any(x=>x.Id.Equals(currentdb.Anexos[i].Id) &&  String.IsNullOrWhiteSpace(x.Name) )  )
                {
                    currentdb.Anexos.RemoveAt(i);
                    i--;
                }
            }
            currentdb.Anexos.AddRange(entity.Anexos.Where(x => x.Id <= 0));
      
            currentdb.Observacoes = entity.Observacoes;
            currentdb.Total = entity.Total;
            currentdb.CotacaoDolar = entity.CotacaoDolar;
            currentdb.DataCotacao = entity.DataCotacao;
            currentdb.AprovacaoAutomatica = entity.AprovacaoAutomatica;

            return await base.Update(currentdb, transacao);
        }

        public IQueryable<SolicitacaoAnexos> QueryAnexos(Expression<Func<SolicitacaoAnexos, bool>> predicate = null)
        {
            var query = this.Context.SolicitacaoAnexos.AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query;

        }

        public DbSet<SolicitacaoHistorico> SetHistorico()
        {
            return this.Context.SolicitacaoHistorico;
        }
    }
}
