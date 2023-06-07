using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    public class ProdutoTipoDto
    { 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
      
        public bool Ativo { get; set; }
        public bool HabilitaAnexos { get; set; }
        public bool HabilitaValoresDolar { get; set; }
        public bool CancelamentoSolicitacaoAutomatico { get; set; }
        public decimal IncrementoQuantidade { get; set; }
        public bool ImprimirConferencia { get; set; }
        public bool PermitirRecorrencia { get; set; }
        public bool PermitirApenasUmItem { get; set; }
        public bool CalcularDiarias { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public int QtdProdutos { get; set; }
        public int QtdSolicitacoesPendentes { get; set; }
        public virtual IEnumerable<ProdutoTipoGestorDto> Gestores { get; set; } = new List<ProdutoTipoGestorDto>();
        public virtual IEnumerable<ProdutoTipoCamposDto> Campos { get; set; } = new List<ProdutoTipoCamposDto>();
        public virtual IEnumerable<ProdutoTipoUsuarioPerfilDto> Usuarios { get; set; } = new List<ProdutoTipoUsuarioPerfilDto>();
    }
    public class ProdutoTipoGestorDto
    {
        public int UsuarioId { get; set; }
        public int ProdutoTipoId { get; set; }
    }

    public class ProdutoTipoCamposDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ETipoCampo TipoCampo { get; set; }
        public int Ordem { get; set; }
        public int Largura { get; set; }
        public int ProdutoTipoId { get; set; }
        public string Opcoes { get; set; }
        public string Grupo { get; set; }
        public bool Obrigatorio { get; set; }
        public int Posicao { get; set; }
    }

    public class ProdutoTipoUsuarioPerfilDto
    {
        public int ProdutoTipoId { get; set; }
        public EUsuarioPerfil Perfil { get; set; }
        public bool PermiteSolicitar { get; set; }
        public bool PermiteCancelar { get; set; }
        public bool PermiteExcluir { get; set; }
        public bool PermiteAprovar { get; set; }
        public bool PermiteRecusar { get; set; }
        public bool PermiteAlterarAprovado { get; set; }
        public ETipoAleracoes TipoAlteracao { get; set; }

    }

}
