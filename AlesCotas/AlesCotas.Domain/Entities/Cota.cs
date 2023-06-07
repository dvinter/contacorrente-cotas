using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    public class Cota : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Lei { get; set; }
        public DateTime DataInicio { get; set; }
        public decimal Valor { get; set; }
       
        public int? MesLimiteAdiantamento { get; set; }


    }
}
