using AlesCotas.Domain.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    public class Publicacao : IBaseEntity
    {
        [Key]
        public int Id { get; set; }
        [DataType("month")]
        public DateTime MesReferencia { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public int GabineteId { get; set; }
        
        [ForeignKey("GabineteId")]
        public Gabinete? Gabinete { get; set; }
    }


}
