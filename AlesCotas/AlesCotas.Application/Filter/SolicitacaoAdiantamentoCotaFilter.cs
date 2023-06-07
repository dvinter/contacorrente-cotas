using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Filtros.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class SolicitacaoAdiantamentoCotaFilter  : BasePeriodoFilter
    {
        public SolicitacaoAdiantamentoCotaFilter()
        {
           // this.Status = EFiltroStatus.Todos;
        }
        public int? GabineteId { get; set; }
    }
}
