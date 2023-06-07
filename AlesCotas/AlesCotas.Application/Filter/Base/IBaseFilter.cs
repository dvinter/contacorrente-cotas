using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter.Base
{
    public interface IBaseFilter
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int Skip { get; }
        string[] Sort { get; set; }
    }
}
