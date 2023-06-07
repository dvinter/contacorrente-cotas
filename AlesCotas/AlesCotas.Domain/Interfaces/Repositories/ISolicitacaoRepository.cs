using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Interfaces.Repositories
{
    public interface ISolicitacaoRepository : IRepository<Solicitacao>
    {
        public DbSet<SolicitacaoHistorico> SetHistorico();
        public IQueryable<SolicitacaoHistorico> QueryHistorico(Expression<Func<SolicitacaoHistorico, bool>> predicate = null);
        public IQueryable<SolicitacaoItem> QueryItens(Expression<Func<SolicitacaoItem, bool>> predicate = null);

        public IQueryable<SolicitacaoAnexos> QueryAnexos(Expression<Func<SolicitacaoAnexos, bool>> predicate = null);
    }
}
