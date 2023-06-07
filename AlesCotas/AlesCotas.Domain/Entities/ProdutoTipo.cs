using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    public class ProdutoTipo : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool Ativo { get; set; }
        public bool HabilitaAnexos { get; set; }
        public bool HabilitaValoresDolar { get; set; }
        public bool CancelamentoSolicitacaoAutomatico { get; set; }
        public bool PermitirRecorrencia { get; set; }
        public decimal IncrementoQuantidade { get; set; }
        public bool ImprimirConferencia { get; set; }
        public bool PermitirApenasUmItem { get; set; }

        /// <summary>
        /// Irá utilizar o campo de intervalo de datas pra aplicar a quantidade do item - no caso das diárias
        /// </summary>
        public bool CalcularDiarias { get; set; }

        /// <summary>
        /// Não utilizar essa lista para atribuir os gestores ao tipo
        /// </summary>
        public virtual IList<ProdutoTipoGestor> Gestores { get; set; } = new List<ProdutoTipoGestor>();
        
        /// <summary>
        /// Lista de Ids dos gestores que fazem bind com o select na view
        /// </summary>
        [NotMapped]
        public virtual IList<int> GestoresId { get; set; } = new List<int>();
        public virtual IList<ProdutoTipoCampos> Campos { get; set; } = new List<ProdutoTipoCampos>();
        public virtual IList<ProdutoTipoUsuarioPerfil> Usuarios { get; set; } = new List<ProdutoTipoUsuarioPerfil>();
    }
    public class ProdutoTipoGestor
    {
        public int UsuarioId { get; set; }
        public int ProdutoTipoId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
    }
    public class ProdutoTipoCampos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ETipoCampo TipoCampo { get; set; }
        public int Ordem { get; set; }
        [StringLength(100)]
        public string? Grupo { get; set; }

        public int Largura { get; set; }

        public int ProdutoTipoId { get; set; }
        public string? Opcoes { get; set; }

        public bool Obrigatorio { get; set; }
        public int Posicao { get; set; }

    }

    public class ProdutoTipoUsuarioPerfil
    {
        [ForeignKey("ProdutoTipoId")]
        public ProdutoTipo? ProdutoTipo { get; set; }
        public int ProdutoTipoId { get; set; }
        public EUsuarioPerfil Perfil { get; set; }
        [Display(Name ="Permite Solicitar")]
        public bool PermiteSolicitar { get; set; }
        [Display(Name = "Permite Cancelar")]
        public bool PermiteCancelar { get; set; }
        [Display(Name = "Permite Excluir")]
        public bool PermiteExcluir { get; set; }
        [Display(Name = "Permite Recusar")]
        public bool PermiteRecusar { get; set; }
        [Display(Name = "Permite Aprovar")]
        public bool PermiteAprovar { get; set; }

        [Display(Name = "Permite Alteração Após Aprovado")]
        public bool PermiteAlterarAprovado { get; set; }

        public ETipoAleracoes TipoAlteracao { get; set; }

    }
}
