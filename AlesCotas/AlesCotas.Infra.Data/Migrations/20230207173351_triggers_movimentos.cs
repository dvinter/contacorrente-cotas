using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class triggers_movimentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_INSERT_MOVIMENTO ON [MovimentoConta] AFTER
INSERT AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id] , [GabineteId] , [Tipo] , [DataMovimento] , [DataRegistro] , [Valor] , [MovimentoId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor,
       i.Id
FROM INSERTED i; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_UPDATE_MOVIMENTO ON [MovimentoConta] AFTER
UPDATE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1),i.Id
FROM deleted i;
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor,
       i.Id
FROM INSERTED i; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_DELETE_MOVIMENTO ON [MovimentoConta] AFTER
DELETE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1),i.Id
FROM deleted i; END;");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER TR_INSERT_MOVIMENTO;");

            migrationBuilder.Sql(@"DROP TRIGGER TR_UPDATE_MOVIMENTO;");

            migrationBuilder.Sql(@"DROP TRIGGER TR_DELETE_MOVIMENTO;");
        }
    }
}
