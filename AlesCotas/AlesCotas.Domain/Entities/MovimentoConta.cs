using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace AlesCotas.Domain.Entities
{
    public class MovimentoConta : IBaseEntity
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public DateTime DataMovimento { get; set; } = DateTime.Now;
        public DateTime? DataReferenciaCota { get; set; }
        public ETipoMovimento Tipo { get; set; }

        [ForeignKey("GabineteId")]
        public virtual Gabinete? Gabinete { get; set; }
        public int GabineteId { get; set; }
        public decimal Valor { get; set; }
        public ENaturezaMovimento NaturezaMovimento { get; set; }
    }
}
