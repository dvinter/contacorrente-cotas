using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter.Base
{
    public class BasePeriodoFilter : BaseEntityFilter
    {
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
    }
}
