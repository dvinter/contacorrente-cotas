using AlesCotas.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Entity { get; set; }
        public int? RefId { get; set; }
        public DateTime EventDate { get; set; }
        public ETipoEventoLog EventType { get; set; }
        public string? IpAddress { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Usuario? Usuario { get; set; }

    }
}
