using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class view_extrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Create view v_extrato_conta as 
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
	  ,Row_Number() Over (Partition by f.[MovimentoId] order by  f.[SequenciaMovimento] desc) as SequenciaMovimento
	  ,(select coalesce(sum(x.valor),0) from fluxo_conta x where x.GabineteId = f.GabineteId and x.Ano = f.Ano and (x.DataMovimento < f.DataMovimento or (x.DataMovimento = f.DataMovimento and x.Sequencia < f.Sequencia))) as SaldoAnterior
	  ,f.valor + (select coalesce(sum(x.valor),0) from fluxo_conta x where x.GabineteId = f.GabineteId and x.Ano = f.Ano and (x.DataMovimento < f.DataMovimento  or (x.DataMovimento = f.DataMovimento and x.Sequencia < f.Sequencia))) as SaldoAtual
	  ,ROW_NUMBER() Over (Partition by f.[GabineteId], f.Ano order by f.DataMovimento, f.Sequencia) as RowNumber
  FROM [fluxo_conta] f
  left outer join Gabinetes g on g.Id = f.GabineteId
  left outer join Logs l on l.id = (select max(lx.id) from Logs lx where  lx.Entity = 'MovimentoConta' and lx.RefId = f.MovimentoId) 
  left outer join Usuarios u on u.Id = l.UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop view v_extrato_conta");
        }
    }
}
