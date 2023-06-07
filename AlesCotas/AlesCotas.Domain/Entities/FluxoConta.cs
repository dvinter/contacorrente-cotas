using AlesCotas.Domain.Enum;
using AlesCotas.Presentation.Binders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    [NotMapped]
    public class FluxoConta
    {
        public int GabineteId { get; set; }
        public int Sequencia { get; set; }
        public int MovimentoId { get; set; }
        public int SequenciaMovimento { get; set; }
        public ETipoMovimento Tipo { get; set; }
        public string? Descricao { get; set; }
        public int? ParlamentarId { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime DataMovimento { get; set; }
        public decimal Valor { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoAtual { get; set; }
        public decimal SaldoAnteriorAno { get; set; }
        public decimal SaldoAtualAno { get; set; }
        public bool Estorno { get; set; }
    }
}
