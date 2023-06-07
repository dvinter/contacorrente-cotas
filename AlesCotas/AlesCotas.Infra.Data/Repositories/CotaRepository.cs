using AlesCotas.Domain.Filtros.Enum;
using AlesCotas.Domain.Filtros;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Domain.Entities;
using AlesCotas.Infra.Data.Identity;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using AlesCotas.Domain.Interfaces.Repositories.Base;

namespace AlesCotas.Infra.Data.Repositories
{


    public class CotaRepository : Repository<Cota>, ICotaRepository
    {
        private readonly IFluxoContaRepository _fluxoContaRepository;
        public CotaRepository(ApplicationDbContext context, ICurrentUserManager currentUserManager, IFluxoContaRepository fluxoContaRepository) : base(context, currentUserManager)
        {
            _fluxoContaRepository = fluxoContaRepository;
        }

        public IQueryable<Cota> DbSetCotas { get => Context.Cotas; }


        public async Task<int> ProcessarCotas(int cotaId)
        {

            CultureInfo ci = new CultureInfo("en-us");

            var strategy = Context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                int retorno = 0;

                using (var transaction = await this.Context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        DateTime dataReferencia = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
                        string sql = @$"SELECT * from Cotas WITH (ROWLOCK, UPDLOCK, NOWAIT) where Id = {cotaId}";

                        var cota = await Context.Cotas.FromSqlRaw(sql).FirstOrDefaultAsync();
                        var gabinetes = await Context.Gabinetes.Where(x => x.Ativo).ToListAsync();

                        if (cota != null)
                        {
                            var movimentos = await Context.MovimentoConta.Where(x => x.Tipo == Domain.Enum.ETipoMovimento.Cota && x.DataReferenciaCota.HasValue && x.DataReferenciaCota.Value.Date == dataReferencia.Date).ToListAsync();
                            List<MovimentoConta> movimentosInsert = new List<MovimentoConta>(gabinetes.Count);
                            foreach (var gabinete in gabinetes)
                            {
                                decimal totalRecebido = movimentos.Where(x => x.GabineteId.Equals(gabinete.Id)).Select(x => x.Valor).Sum();
                                decimal diferenca = cota.Valor - totalRecebido;

                                movimentosInsert.Add(new MovimentoConta { 
                                    DataMovimento = dataReferencia, 
                                    DataReferenciaCota = dataReferencia,
                                    Descricao = "Processamento de cotas "+ dataReferencia.ToString("dd/MM/yyyy"),
                                    GabineteId = gabinete.Id,
                                     NaturezaMovimento = Domain.Enum.ENaturezaMovimento.Credito,
                                      Tipo = Domain.Enum.ETipoMovimento.Cota,
                                       Valor = diferenca
                                });


                                //DateTime mesAnterior = dataReferencia.AddDays(-1);
                                //var query = await _fluxoContaRepository.QueryExtrato().Where(x => x.GabineteId.Equals(gabinete.Id) && x.DataMovimento.Year == mesAnterior.Year && x.DataMovimento.Month == mesAnterior.Month).OrderByDescending(x=>x.Sequencia).FirstOrDefaultAsync();

                                //if (query != null)
                                //{
                                //    //decimal saldoMesAnterior = query.SaldoAtual;
                                //    //if (saldoMesAnterior > 0)
                                //    //{
                                //    //    movimentosInsert.Add(new MovimentoConta
                                //    //    {
                                //    //        DataMovimento = mesAnterior,
                                //    //        DataReferenciaCota = null,
                                //    //        Descricao = "Transferência Saldo restante " + mesAnterior.ToString("MMM/yyyy"),
                                //    //        GabineteId = gabinete.Id,
                                //    //        NaturezaMovimento = Domain.Enum.ENaturezaMovimento.Debito,
                                //    //        Tipo = Domain.Enum.ETipoMovimento.TransferenciaSaldo,
                                //    //        Valor = saldoMesAnterior

                                //    //    });

                                //    //    movimentosInsert.Add(new MovimentoConta
                                //    //    {
                                //    //        DataMovimento = dataReferencia,
                                //    //        DataReferenciaCota = null,
                                //    //        Descricao = "Transferência de Saldo restante " + mesAnterior.ToString("MMM/yyyy"),
                                //    //        GabineteId = gabinete.Id,
                                //    //        NaturezaMovimento = Domain.Enum.ENaturezaMovimento.Credito,
                                //    //        Tipo = Domain.Enum.ETipoMovimento.TransferenciaSaldo,
                                //    //        Valor = saldoMesAnterior
                                //    //    });
                                //    //}
                                //}
                            }

                            await this.Context.MovimentoConta.AddRangeAsync(movimentosInsert);

                            await this.Context.Logs.AddAsync(new Log { Entity = "System", Description = "Processamento de cotas " + dataReferencia.ToString("dd/MM/yyyy"), EventDate = DateTime.Now, EventType = Domain.Enum.ETipoEventoLog.ProcessamentoCotas, IpAddress = null, RefId = null, UserId = null });

                            this.Context.SaveChanges();
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
                return retorno;
            });

        }
   
    }
}