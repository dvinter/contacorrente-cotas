using AlesCotas.Domain.Filtros.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Filter
{
    public class ExtratoFilter 
    {
        public int GabineteId { get; set; }
        [BindProperty, DataType("month")]
        public DateTime MesInicio { get; set; } = DateTime.Now;
        [BindProperty, DataType("month")]
        public DateTime MesFim { get; set; } = DateTime.Now;

        public EFiltroTipoData TipoData { get; set; }


    }
}
