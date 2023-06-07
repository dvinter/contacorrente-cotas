using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    
    public class CotaDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Lei { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DataInicio { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DataFim { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        public string UsuarioAtualizacao { get; set; }
        public decimal Valor { get; set; }
        public int? MesLimiteAdiantamento { get; set; }
        public bool Ativo { get => DataInicio <= DateTime.Now && ((DataFim.HasValue && DataFim.Value > DateTime.Now) || !DataFim.HasValue); }

    }
}
