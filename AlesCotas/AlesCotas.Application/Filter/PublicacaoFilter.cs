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
    public class PublicacaoFilter : BaseEntityFilter
    {
        public int GabineteId { get; set; }
        [BindProperty, DataType("month")]
        public DateTime MesReferencia { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1).AddMonths(-1);

    }
}
