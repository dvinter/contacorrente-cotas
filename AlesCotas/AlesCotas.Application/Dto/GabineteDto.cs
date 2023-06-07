using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Dto
{
    public class GabineteDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Ativo { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }
        [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
        public decimal? SaldoDisponivel { get => SaldoAtualx - SaldoReservado; }
        public decimal? SaldoReservado { get; set; }
        public decimal? SaldoAtualx { get; set; }

    }
}

