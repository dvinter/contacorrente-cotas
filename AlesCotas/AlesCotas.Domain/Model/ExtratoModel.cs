using AlesCotas.Domain.Enum;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Model
{

    [NotMapped]
    public class ExtratoViewModel
    {
        public decimal SaldoAtual { get; set; }
        public decimal SaldoReservado { get; set; }
        public decimal SaldoDisponivel { get => SaldoAtual - SaldoReservado; }
        public List<ExtratoModel> Movimentos { get; set; } 
    }

    [NotMapped]
    public class ExtratoModel
    {
        public long RowNumber { get; set; }
        public int Sequencia { get; set; }
        public int MovimentoId { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataMovimento { get; set; }
        public DateTime DataRegistro { get; set; }
        public string? UsuarioRegistro { get; set; }
        public int GabineteId { get; set; }
        public string? GabineteName { get; set; }
        public string? Descricao { get; set; }
        public ETipoMovimento Tipo { get; set; }
        public string TipoDescricao { get => this.Tipo.GetEnumDescription(); }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal Valor { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal SaldoAnterior { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal SaldoAtual { get; set; }
        public long SequenciaMovimento { get; set; }
        public bool Estorno { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }

    }

    public class ExtratoTotalModel
    {
        public int GabineteId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public ETipoMovimento Tipo { get; set; }
        public decimal Total { get; set; }
             
    }
}
