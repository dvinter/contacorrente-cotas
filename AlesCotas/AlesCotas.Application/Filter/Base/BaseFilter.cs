using AlesCotas.Domain.Filtros.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter.Base
{
 

    public class BaseFilter : IBaseFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public int Skip { get => PageSize * (PageNumber - 1); }

        public string[] Sort { get; set; }
    }

    public class BaseEntityFilter : BaseFilter
    {
        public EFiltroStatus Status { get; set; } = EFiltroStatus.Ativo;
        public string FilterText { get; set; }
    }
}
