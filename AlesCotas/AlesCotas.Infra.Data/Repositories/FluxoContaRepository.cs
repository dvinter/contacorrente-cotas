using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Domain.Model;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.SqlQuery;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.Data.Repositories
{
    public class FluxoContaRepository : IDisposable, IFluxoContaRepository
    {
        private readonly ApplicationDbContext _context;
        public FluxoContaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        public async Task<int> ProcessarFila()
        {

            if (this._context.FluxoContaFila.Any())
            {
                CultureInfo ci = new CultureInfo("en-us");

                var idsSelect = await this._context.FluxoContaFila.AsNoTracking().OrderBy(x => x.DataRegistro).Take(10).Select(x => x.Id).ToListAsync();

                var strategy = this._context.Database.CreateExecutionStrategy();

                return await strategy.ExecuteAsync(async () =>
                {
                    int retorno = 0;

                    using (var transaction = await this._context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                    {
                        try
                        {
                            string sql = @"SELECT * from fluxo_conta_fila WITH (ROWLOCK, UPDLOCK, NOWAIT) where Id in ('" + String.Join("','", idsSelect) + "') order by [DataRegistro],[MovimentoId],[Sequencia] ;"; // in (select top 2 id from " + GetTableFilaName() + " order by DataRegistro);";

                            var fila = await _context.FluxoContaFila.FromSqlRaw(sql).ToListAsync();

                            if (fila.Count > 0)
                            {
                                List<Guid> ids = new List<Guid>();
                                foreach (var item in fila)
                                {
                                    string SqlInser = @$"
     INSERT INTO [fluxo_conta]
           ([GabineteId]
           ,[Sequencia]
           ,[MovimentoId]
           ,[SequenciaMovimento]
           ,[Ano]
           ,[Mes]
           ,[DataRegistro]
           ,[DataMovimento]
           ,[Valor]
           ,[SaldoAnterior]
           ,[SaldoAtual]
           ,[SaldoAnteriorAno]
           ,[SaldoAtualAno]
           ,[Descricao]
           ,[Tipo]
           ,[Estorno]
            )
     VALUES
           ('{item.GabineteId}'
           ,(select coalesce(max(x.Sequencia),0)+1 from [fluxo_conta] x where x.GabineteId = {item.GabineteId})
           ,{item.MovimentoId}
           ,(select coalesce(max(x.SequenciaMovimento),0)+1 from [fluxo_conta] x where x.MovimentoId = {item.MovimentoId})
           ,{item.DataMovimento.Year}
           ,{item.DataMovimento.Month}
           ,@dataregistro
           ,@datamovimento
           ,{item.Valor.ToString("G", ci)}
           ,coalesce((SELECT top 1 x.SaldoAtual from [fluxo_conta] x where x.[GabineteId] = {item.GabineteId} and x.[Ano] = {item.DataMovimento.Year} and x.[Mes] = {item.DataMovimento.Month} order by x.[Sequencia] desc),0)
           ,(coalesce((SELECT top 1 x.SaldoAtual from [fluxo_conta] x where x.[GabineteId] = {item.GabineteId} and x.[Ano] = {item.DataMovimento.Year} and x.[Mes] = {item.DataMovimento.Month} order by x.[Sequencia] desc),0) + {item.Valor.ToString("G", ci)})
           ,coalesce((SELECT top 1 x.SaldoAtualAno from [fluxo_conta] x where x.[GabineteId] = {item.GabineteId} and x.[Ano] = {item.DataMovimento.Year} order by x.[Sequencia] desc),0)
           ,(coalesce((SELECT top 1 x.SaldoAtualAno from [fluxo_conta] x where x.[GabineteId] = {item.GabineteId} and x.[Ano] = {item.DataMovimento.Year} order by x.[Sequencia] desc),0) + {item.Valor.ToString("G", ci)})
           ,'{item.Descricao}'
           ,{(int)item.Tipo}
           ,{(item.Estorno? 1:0)}
);";

                                    var parameters = new List<SqlParameter>()
                                    {
                                        new SqlParameter("@dataregistro", item.DataRegistro),
                                        new SqlParameter("@datamovimento", item.DataMovimento)
                                    };

                                    retorno += await _context.Database.ExecuteSqlRawAsync(SqlInser,parameters);
                                    ids.Add(item.Id);

                                }

                                string sqlDelete = @$"DELETE FROM [fluxo_conta_fila] WHERE [Id] in ('{String.Join("','", ids)}');";

                                

                                retorno += await _context.Database.ExecuteSqlRawAsync(sqlDelete);


                                this._context.SaveChanges();
                                await transaction.CommitAsync();
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                            }
                        }
                        catch
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                    return retorno;
                });
            }
            return 0;
        }


        public IQueryable<ExtratoTotalModel> QueryTotais()
        {
            return this._context.Set<ExtratoTotalModel>();
        }
        public IQueryable<ExtratoModel> QueryExtratoDataMovimento()
        {
            return this._context.Set<ExtratoModel>().FromSqlRaw(SqlQueryExtratoView.QUERY_FLUXO_DATA_MOVIMENTO);
        }
        public IQueryable<ExtratoModel> QueryExtratoDataRegistro()
        {
            return this._context.Set<ExtratoModel>().FromSqlRaw(SqlQueryExtratoView.QUERY_FLUXO_DATA_REGISTRO);
        }
        public IQueryable<TotalSolicitacoesModel> QueryTotalSolicitacoes()
        {
            return this._context.Set<TotalSolicitacoesModel>();
        }
    }
}
