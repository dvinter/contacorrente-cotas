using AlesCotas.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Model
{
    [NotMapped]
    public class TotalSolicitacoesModel
    {
        public int GabineteId { get; set; }
        public DateTime MesReferencia { get; set; }
        public decimal TotalSolicitacao { get; set; }
        public ESolicitacaoStatus Status { get; set; }
    }
}
