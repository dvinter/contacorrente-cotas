using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using System.ComponentModel.DataAnnotations;

namespace AlesCotas.Application.Dto
{
    public class MovimentoContaDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public ETipoMovimento? Tipo { get; set; }
        public string TipoDescricao { get => this.Tipo.GetEnumDescription();  }
        public int? GabineteId { get; set; }
        public Gabinete? Gabinete { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DataMovimento { get; set; }
        public DateTime? DataReferenciaCota { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public decimal Valor { get; set; }
    }
}
