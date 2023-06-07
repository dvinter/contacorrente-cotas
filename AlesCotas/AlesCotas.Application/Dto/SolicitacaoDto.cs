using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AlesCotas.Application.Dto
{
    public class SolicitacaoDto
    {
        public int Id { get; set; }
        public string Observacoes { get; set; }
        public string Numero { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int GabineteId { get; set; }
        public string GabineteNome { get; set; }
     
        public int ProdutoTipoId { get; set; }
        public string ProdutoTipoDescricao { get; set; }
        public bool HabilitaValoresDolar { get; set; }
        public string GabineteDescricao { get => this.GabineteNome; }
        public DateTime? UltimaAtualizacao { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = false)]
        public decimal TotalSolicitado { get; set; }
        public int QtdItens { get; set; }
        public bool Publicado { get; set; }
        public SolicitacaoHistoricoDto? StatusAtual { get; set; }
        
        public DateTime? DataCotacao { get; set; }

        public decimal? CotacaoDolar { get; set; }
        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = false)]
        public decimal? TotalDolar { get => !this.CotacaoDolar.HasValue ? null : this.CotacaoDolar == 0 ? this.TotalSolicitado : Math.Round((this.TotalSolicitado / this.CotacaoDolar.Value), 2); }

    }

    public class SolicitacaoHistoricoDto
    {
        public int Sequencia { get; set; }
        public int SolicitacaoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public int UsuarioRegistroId { get; set; }
        public ESolicitacaoStatus Status { get; set; }
        public string StatusDescricao { get => this.Status.GetEnumDescription(); }

    }
}

