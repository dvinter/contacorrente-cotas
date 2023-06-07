using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class fluxo_campos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "fluxo_conta_fila",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "fluxo_conta_fila",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "fluxo_conta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParlamentarId",
                table: "fluxo_conta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "fluxo_conta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_INSERT_MOVIMENTO ON [MovimentoConta] AFTER
INSERT AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id] , [GabineteId] , [Tipo] , [DataMovimento] , [DataRegistro] , [Valor] , [MovimentoId], [Descricao], [ParlamentarId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor,
       i.Id,
       i.Descricao,
       g.ParlamentarId
FROM INSERTED i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_UPDATE_MOVIMENTO ON [MovimentoConta] AFTER
UPDATE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       g.ParlamentarId
FROM deleted i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
;
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),
       i.Valor,
       i.Id,
       i.Descricao,
       g.ParlamentarId
FROM INSERTED i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId
; END;");

            migrationBuilder.Sql(@"
CREATE
OR
ALTER TRIGGER TR_DELETE_MOVIMENTO ON [MovimentoConta] AFTER
DELETE AS BEGIN
INSERT INTO [dbo].[fluxo_conta_fila] ([Id], [GabineteId], [Tipo], [DataMovimento], [DataRegistro], [Valor], [MovimentoId], [Descricao], [ParlamentarId])
SELECT NEWID(),
       i.GabineteId,
       i.Tipo,
       i.DataMovimento,
       GETDATE(),(i.Valor*-1),i.Id,
       CONCAT('ESTORNO',' ', i.Descricao),
       g.ParlamentarId
FROM deleted i
LEFT OUTER JOIN gabinetes g on g.Id = i.GabineteId; END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "fluxo_conta_fila");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "fluxo_conta_fila");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "fluxo_conta");

            migrationBuilder.DropColumn(
                name: "ParlamentarId",
                table: "fluxo_conta");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "fluxo_conta");
        }
    }
}
