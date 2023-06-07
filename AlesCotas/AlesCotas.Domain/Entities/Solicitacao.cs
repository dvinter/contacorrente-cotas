using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlesCotas.Domain.Entities
{
    public class Solicitacao : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string? Numero { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string? Observacoes { get; set; }
        public int GabineteId { get; set; }
        public int ProdutoTipoId { get; set; }
        [ForeignKey("ProdutoTipoId")]
        public ProdutoTipo? ProdutoTipo { get; set; }
        [ForeignKey("GabineteId")]
        public Gabinete? Gabinete { get; set; }
        public virtual List<SolicitacaoItem> Items { get; set; } = new List<SolicitacaoItem>();
        public virtual List<SolicitacaoHistorico> Historico { get; set; } = new List<SolicitacaoHistorico>();
        public virtual List<SolicitacaoCampos> Campos { get; set; } = new List<SolicitacaoCampos>();
        public virtual List<SolicitacaoAnexos> Anexos { get; set; } = new List<SolicitacaoAnexos>();
        public decimal Total { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal CotacaoDolar { get; set; }

        public DateTime? DataCotacao { get; set; }

        [NotMapped]
        public decimal TotalDolar { get => this.CotacaoDolar == 0 ? this.Total : Math.Round((this.Total / this.CotacaoDolar),2); }

        [NotMapped]
        public IFormFile[] FileAnexo { get; set; }
        
        [NotMapped]
        public string? NovaJustificativa { get; set; }

        public bool AprovacaoAutomatica { get; set; }
    }

    public class SolicitacaoItem
    {
        public int Sequencia { get; set; }
        public int SolicitacaoId { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto? Produto { get; set; }
        public int ProdutoId { get; set; }
        public decimal Valor { get; set; }
        public decimal QuantidadeSolicitada { get; set; }

        [NotMapped]
        public decimal Total { get => Valor * QuantidadeSolicitada; }
    }

    public class SolicitacaoHistorico
    {
        public int Sequencia { get; set; }
        public int SolicitacaoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public int UsuarioRegistroId { get; set; }
        [ForeignKey("UsuarioRegistroId")]
        public Usuario? UsuarioRegistro { get; set; }
        public ESolicitacaoStatus Status { get; set; }

        public string? Justificativa { get; set; }

    }

    public class SolicitacaoAnexos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extensao { get; set; }
        public DateTime DataArquivo { get; set; }
        public long Tamanho { get; set; }
        public byte[] Dados { get; set; }
        public int SolicitacaoId { get; set; }
    }

    public class SolicitacaoCampos
    {
        public int SolicitacaoId { get; set; }
        public string Name { get; set; }
        public ETipoCampo TipoCampo { get; set; }
        public int Ordem { get; set; }
        public int Largura { get; set; }
        public string? Valor { get; set; }
        public int Posicao { get; set; }

        [NotMapped]
        public bool BoolValor
        {
            get
            {
                if (bool.TryParse(this.Valor, out bool b))
                    return b;
                else if (this.Valor == "1" || this.Valor == "Sim")
                    return true;

                return false;
            }
            set
            {
                if (TipoCampo == ETipoCampo.SimNao)
                {
                    if (value)
                        this.Valor = "1";
                    else
                        this.Valor = "0";
                }
            }
        }

        [NotMapped]
        [BindProperty, DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateTimeValor
        {
            get
            {
                if (DateTime.TryParse(this.Valor, out DateTime b))
                    return b;
                return null;
            }
            set
            {
                if (TipoCampo == ETipoCampo.DataHora)
                {
                    this.Valor = value.HasValue ? value.Value.ToString() : null;
                }
            }
        }

        [NotMapped]
        public DateTime?[] IntervaloDatas
        {
            get
            {
                DateTime?[] datas = new DateTime?[2] { null,null};
                if (this.Valor != null)
                {
                    string[] sdatas = this.Valor.Split(" a ");
                    if (TipoCampo == ETipoCampo.IntervaloDatasHoras)
                    {
                        if (sdatas?.Length >= 1 && DateTime.TryParseExact(sdatas[0], "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime dt1))
                        {
                            datas[0] = dt1;
                        }
                        if (sdatas?.Length >= 2 && DateTime.TryParseExact(sdatas[1], "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime dt2))
                        {
                            datas[1] = dt2;
                        }
                    }
                    else
                    {
                        if (sdatas?.Length >= 1 && DateTime.TryParse(sdatas[0], out DateTime dt1))
                        {
                            datas[0] = dt1;
                        }
                        if (sdatas?.Length >= 2 && DateTime.TryParse(sdatas[1], out DateTime dt2))
                        {
                            datas[1] = dt2;
                        }
                    }
                }
                return datas;
            }
            set
            {
                if (TipoCampo == ETipoCampo.IntervaloDatas)
                {
                    this.Valor = value != null ?  String.Join(" a ", value.Select(x => x?.ToString("dd/MM/yyyy"))) : null;
                }
                else if (TipoCampo == ETipoCampo.IntervaloDatasHoras)
                {
                    this.Valor = value != null ? String.Join(" a ", value.Select(x => x?.ToString("dd/MM/yyyy HH:mm"))) : null;
                }
            }
        }

        [NotMapped]
        public string Grupo { get; set; }
        [NotMapped]
        
        public string Opcoes { get; set; }
        [NotMapped]
        public bool Obrigatorio { get; set; }

    }
}

