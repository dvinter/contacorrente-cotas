using AlesCotas.Application.Filter.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class ProdutoFilter : BaseEntityFilter
    {
        public int ProdutoTipoId { get; set; }
    }
}
