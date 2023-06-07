using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.Data.SqlQuery
{
    public static class SqlQueryTotalSolicitacoesView
    {
        /// <summary>
        /// Query pro total de solicitações por gabinete, mês e status
        /// </summary>
        public const string QUERY = @"
  select
  s.GabineteId,
  DATEADD(month, DATEDIFF(month, 0, s.DataSolicitacao), 0) as MesReferencia,
  sum(coalesce(s.Total,0)) as TotalSolicitacao,
  sh.Status
  from Solicitacao s
  inner join SolicitacaoHistorico sh on sh.SolicitacaoId = s.Id and sh.Sequencia = (select max(x.sequencia) from SolicitacaoHistorico x where x.SolicitacaoId = s.Id)
  group by s.GabineteId,
   DATEADD(month, DATEDIFF(month, 0, s.DataSolicitacao), 0),
   sh.Status
";
    }
}
