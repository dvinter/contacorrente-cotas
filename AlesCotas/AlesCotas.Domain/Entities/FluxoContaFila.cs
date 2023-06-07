using AlesCotas.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace AlesCotas.Domain.Entities
{
    public class FluxoContaFila
    {
        [Key]
        public Guid Id { get; set; }
        public int GabineteId { get; set; }
        public string? Descricao { get; set; }
        public ETipoMovimento Tipo { get; set; }
        public DateTime DataMovimento { get; set; }
        public DateTime DataRegistro { get; set; }
        public decimal Valor { get; set; }
        public int MovimentoId { get; set; }
        public int Sequencia { get; set; }
        public bool Estorno { get; set; }

    }
}
