using AlesCotas.Domain.Enum;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Model
{

    [NotMapped]
    public class ServidorViewModel
    {
        public string? MatriculaVinculo { get; set; }
        public string? NomeServidor { get; set; }
        public string? Regime { get; set; }
        public string? PadraoSalarial { get; set; }
        public string? CargaHoraria { get; set; }
        public string? Cargo { get; set; }
        public string? Setor { get; set; }
        public string? DataAdmissao { get; set; }
        public string? DataDemissao { get; set; }
        public string? EmailServidor { get; set; }
        public bool erroConexao { get; set; }
        public string? erroMsg { get; set; }

    }
}
