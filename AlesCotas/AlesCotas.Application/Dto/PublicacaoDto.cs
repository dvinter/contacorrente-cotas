using AlesCotas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    public class PublicacaoDto
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMM/yyyy}")]
        public DateTime MesReferencia { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int GabineteId { get; set; }
        public string GabineteName { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
    }
}
