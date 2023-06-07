using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class remove_parlamentar_fluxoFila : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "fluxo_conta_fila");


            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_INSERT_MOVIMENTO ON [MovimentoConta] AFTER
INSERT AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id] , [GabineteId] , [Tipo] , [DataMovimento] , [DataRegistro] , [Valor] , [MovimentoId], [Descricao], [Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor * (case when i.NaturezaMovimento = 0 then 1 else -1 end),
       i.Id,
       i.Descricao,
       1,
       0       
FROM INSERTED i
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_UPDATE_MOVIMENTO ON [MovimentoConta] AFTER
UPDATE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1) * (case when i.NaturezaMovimento = 0 then 1 else -1 end),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       1,
       1
FROM deleted i
;
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor * (case when i.NaturezaMovimento = 0 then 1 else -1 end),
       i.Id,
       i.Descricao,
       2,
       0
FROM INSERTED i
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_DELETE_MOVIMENTO ON [MovimentoConta] AFTER
DELETE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao],[Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1)* (case when i.NaturezaMovimento = 0 then 1 else -1 end),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       1,
       1
FROM deleted i; END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "fluxo_conta_fila",
                type: "int",
                nullable: true);


            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_INSERT_MOVIMENTO ON [MovimentoConta] AFTER
INSERT AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id] , [GabineteId] , [Tipo] , [DataMovimento] , [DataRegistro] , [Valor] , [MovimentoId], [Descricao], [ParlamentarId],[Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor * (case when i.NaturezaMovimento = 0 then 1 else -1 end),
       i.Id,
       i.Descricao,
       g.ParlamentarId,
       1,
       0       
FROM INSERTED i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_UPDATE_MOVIMENTO ON [MovimentoConta] AFTER
UPDATE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId],[Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1) * (case when i.NaturezaMovimento = 0 then 1 else -1 end),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       g.ParlamentarId,
       1,
       1
FROM deleted i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
;
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId],[Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor * (case when i.NaturezaMovimento = 0 then 1 else -1 end),
       i.Id,
       i.Descricao,
       g.ParlamentarId,
       2,
       0
FROM INSERTED i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_DELETE_MOVIMENTO ON [MovimentoConta] AFTER
DELETE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId],[Sequencia],[Estorno])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1)* (case when i.NaturezaMovimento = 0 then 1 else -1 end),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       g.ParlamentarId,
       1,
       1
FROM deleted i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId; END;");
        }
    }
}
