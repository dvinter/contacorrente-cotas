using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class ProdutoTipoFilter : BaseEntityFilter
    {
        public ProdutoTipoFilter()
        {
            this.PageSize = 15;




        }

        public Expression<Func<ProdutoTipo, bool>> FilterExpression { get; set; } = null;
    }
}
