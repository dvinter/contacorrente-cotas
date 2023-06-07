using AlesCotas.Application.Filter.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class GabineteFilter : BaseEntityFilter
    {
        public int? GabineteId { get; set; }
    }
}
