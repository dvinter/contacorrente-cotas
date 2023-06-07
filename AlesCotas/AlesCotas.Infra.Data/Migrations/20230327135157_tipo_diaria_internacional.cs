using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class tipo_diaria_internacional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"

SET IDENTITY_INSERT [dbo].[ProdutoTipo] ON
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId],[HabilitaValoresDolar]) VALUES (10, N'Diárias Internacionais', N'request-calendar.svg', N'UN', 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (10, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (10, 2, 0, 1, 1, 1, 2)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (10, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (10, 4, 0, 0, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[ProdutoTipo] OFF

SET IDENTITY_INSERT [dbo].[ProdutoTipoCampos] ON
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (24, N'Favorecido', 0, 1, 10, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (25, N'Tipo Favorecido', 9, 2, 10, 6, N'Deputado,Gabinete', NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (26, N'Motivação Pública', 1, 3, 10, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (27, N'Motivo da Viagem', 1, 4, 10, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (28, N'Local de Hospedagem', 0, 5, 10, 12, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (29, N'Data de Partida', 4, 6, 10, 6, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (30, N'Data de Retorno', 4, 7, 10, 6, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (31, N'Tipo Pagamento', 9, 8, 10, 12, N'Ordem Bancária,PIX', N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (32, N'Titular da Conta', 0, 9, 10, 12, NULL, N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (33, N'Documento do Titular', 0, 10, 10, 12, NULL, N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (34, N'Dados do Pagamento', 1, 11, 10, 12, NULL, N'Pagamento')
GO
SET IDENTITY_INSERT [dbo].[ProdutoTipoCampos] OFF
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
