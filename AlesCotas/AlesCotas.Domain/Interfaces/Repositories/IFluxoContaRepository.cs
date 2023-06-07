using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Interfaces.Repositories
{
    public interface IFluxoContaRepository
    {
        IQueryable<ExtratoModel> QueryExtratoDataRegistro();
        IQueryable<ExtratoModel> QueryExtratoDataMovimento();
        IQueryable<ExtratoTotalModel> QueryTotais();
        IQueryable<TotalSolicitacoesModel> QueryTotalSolicitacoes();
        Task<int> ProcessarFila();
    }
}
