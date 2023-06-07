using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlesCotas.Infra.Data.Migrations
{
    public partial class insert_produto_tipos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
SET IDENTITY_INSERT [dbo].[ProdutoTipo] ON 
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (1, N'Combustível', N'request-fuel.svg', N'L', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (2, N'Postagens', N'request-post.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (3, N'Postagem AR', N'post-ar.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (4, N'Diárias', N'request-calendar.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (5, N'Passagem Aérea', N'request-plane.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (6, N'Material Expediente', N'request-material.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (7, N'Cópia Reprográfica', N'request-copy.svg', N'UN', 1, 1, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (8, N'Telefonia', N'telephone.svg', N'UN', 1, 0, 0, 0, NULL)
GO
INSERT [dbo].[ProdutoTipo] ([Id], [Name], [Icon], [Unidade], [Ativo], [HabilitaAnexos], [CancelamentoSolicitacaoAutomatico], [NumeroParcelas], [GestorId]) VALUES (9, N'Periódicos', N'request-periodicals.svg', N'UN', 1, 0, 0, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[ProdutoTipo] OFF
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (1, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (1, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (1, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (1, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (2, 1, 1, 0, 0, 0, 9)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (2, 2, 0, 1, 1, 1, 9)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (2, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (2, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (3, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (3, 2, 0, 1, 1, 1, 9)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (3, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (3, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (4, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (4, 2, 0, 1, 1, 1, 2)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (4, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (4, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (5, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (5, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (5, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (5, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (6, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (6, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (6, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (6, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (7, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (7, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (7, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (7, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (8, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (8, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (8, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (8, 4, 0, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (9, 1, 1, 0, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (9, 2, 0, 1, 1, 1, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (9, 3, 1, 1, 0, 0, 0)
GO
INSERT [dbo].[ProdutoTipoUsuarioPerfil] ([ProdutoTipoId], [Perfil], [PermiteSolicitar], [PermiteCancelar], [PermiteExcluir], [PermiteAprovar], [TipoAlteracao]) VALUES (9, 4, 0, 0, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[ProdutoTipoCampos] ON 
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (1, N'Favorecido', 0, 1, 4, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (2, N'Tipo Favorecido', 9, 2, 4, 6, N'Deputado,Gabinete', NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (3, N'Motivação Pública', 1, 3, 4, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (4, N'Motivo da Viagem', 1, 4, 4, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (5, N'Local de Hospedagem', 0, 5, 4, 12, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (6, N'Data de Partida', 4, 6, 4, 6, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (7, N'Data de Retorno', 4, 7, 4, 6, NULL, N'Intinerário')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (8, N'Tipo Pagamento', 9, 8, 4, 12, N'Ordem Bancária,PIX', N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (9, N'Titular da Conta', 0, 9, 4, 12, NULL, N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (10, N'Documento do Titular', 0, 10, 4, 12, NULL, N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (11, N'Dados do Pagamento', 1, 11, 4, 12, NULL, N'Pagamento')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (12, N'Tipo Favorecido', 9, 1, 5, 12, N'Deputado,Gabinete', NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (13, N'Favorecido', 0, 2, 5, 6, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (14, N'Empresa aérea', 0, 3, 5, 6, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (15, N'Motivo da Viagem', 1, 4, 5, 12, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (16, N'Origem', 0, 5, 5, 6, NULL, N'Informações da Passagem')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (17, N'Destino', 0, 6, 5, 6, NULL, N'Informações da Passagem')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (18, N'Partida', 4, 7, 5, 6, NULL, N'Informações da Passagem')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (19, N'Retorno', 4, 8, 5, 6, NULL, N'Informações da Passagem')
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (20, N'Grampeamento', 9, 1, 7, 6, N'Sim,Não', NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (21, N'Início', 4, 1, 8, 6, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (22, N'Término', 4, 2, 8, 6, NULL, NULL)
GO
INSERT [dbo].[ProdutoTipoCampos] ([Id], [Name], [TipoCampo], [Ordem], [ProdutoTipoId], [Largura], [Opcoes], [Grupo]) VALUES (23, N'Tipo', 9, 1, 9, 12, N'Impresso,Digital', NULL)
GO
SET IDENTITY_INSERT [dbo].[ProdutoTipoCampos] OFF
GO
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
