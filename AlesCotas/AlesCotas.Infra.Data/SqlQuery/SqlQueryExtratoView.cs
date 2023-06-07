using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.Data.SqlQuery
{
    public static class SqlQueryExtratoView
    {
        /// <summary>
        /// Select de visualização do fluxo da conta do gabinete
        /// </summary>
        public const string QUERY_FLUXO_DATA_MOVIMENTO = @"

    SELECT f.[GabineteId]
          ,f.[Sequencia]
          ,f.[MovimentoId]
          ,f.[Ano]
          ,f.[Mes]
          ,f.[DataRegistro]
          ,f.[DataMovimento]
          ,f.[Valor]
          ,f.[Descricao]
          ,f.[ParlamentarId]
    	  ,f.[Tipo]
          ,f.[Estorno]
    	  ,g.Name as GabineteName
    	  ,coalesce(u.Nome,'Sistema') as UsuarioRegistro
		  ,(select coalesce(sum(x.Valor),0) from [fluxo_conta] x where x.GabineteId = f.GabineteId and x.Ano = f.Ano and (x.DataMovimento < f.DataMovimento OR (x.DataMovimento = f.DataMovimento AND x.Sequencia < f.Sequencia))) as SaldoAnterior
		  ,f.Valor + (select coalesce(sum(x.Valor),0) from [fluxo_conta] x where x.GabineteId = f.GabineteId and x.Ano = f.Ano and (x.DataMovimento < f.DataMovimento OR (x.DataMovimento = f.DataMovimento AND x.Sequencia < f.Sequencia))) as SaldoAtual
    	  ,Row_Number() Over (Partition by f.[MovimentoId] order by  f.[SequenciaMovimento] desc) as SequenciaMovimento
    	  ,ROW_NUMBER() Over (Partition by f.[GabineteId], f.Ano order by f.DataMovimento, f.Sequencia) as RowNumber
      FROM [fluxo_conta] f
      left outer join Gabinetes g on g.Id = f.GabineteId
      left outer join Logs l on l.id = (select max(lx.id) from Logs lx where  lx.Entity = 'MovimentoConta' and lx.RefId = f.MovimentoId) 
      left outer join Usuarios u on u.Id = l.UserId
";

        public const string QUERY_FLUXO_DATA_REGISTRO = @"

    SELECT f.[GabineteId]
          ,f.[Sequencia]
          ,f.[MovimentoId]
          ,f.[Ano]
          ,f.[Mes]
          ,f.[DataRegistro]
          ,f.[DataMovimento]
          ,f.[Valor]
          ,f.[Descricao]
          ,f.[ParlamentarId]
    	  ,f.[Tipo]
          ,f.[Estorno]
    	  ,g.Name as GabineteName
    	  ,coalesce(u.Nome,'Sistema') as UsuarioRegistro
		  ,f.SaldoAnteriorAno as SaldoAnterior
		  ,f.SaldoAtualAno as SaldoAtual
    	  ,Row_Number() Over (Partition by f.[MovimentoId] order by  f.[SequenciaMovimento] desc) as SequenciaMovimento
    	  ,ROW_NUMBER() Over (Partition by f.[GabineteId], f.Ano order by f.Sequencia) as RowNumber
      FROM [fluxo_conta] f
      left outer join Gabinetes g on g.Id = f.GabineteId
      left outer join Logs l on l.id = (select max(lx.id) from Logs lx where  lx.Entity = 'MovimentoConta' and lx.RefId = f.MovimentoId) 
      left outer join Usuarios u on u.Id = l.UserId
";

        /// <summary>
        /// Query de total de movimentos por gabinete, ano e mês
        /// </summary>
        public const string QUERY_TOTAIS = @"
SELECT f.[GabineteId]
      ,f.[Ano]
      ,f.[Mes]
      ,f.[Tipo]
	  ,sum(f.[Valor]) as Total
  FROM [fluxo_conta] f
  group by
  f.[GabineteId]
      ,f.[Ano]
      ,f.[Mes]
      ,f.[Tipo]
";
    }
}
