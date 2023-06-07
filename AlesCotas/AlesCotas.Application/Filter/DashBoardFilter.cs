using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class DashBoardFilter
    {
        public int ProdutoTipoId { get; set; }
        public int GabineteId { get; set; }
        public DateTime DataInicio { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);
        public DateTime DataFim { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    }
}
