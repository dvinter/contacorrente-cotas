using AlesCotas.Application.Dto;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Model
{
    public class DashBoardViewModel
    {
        public List<TotalSolicitacoesModel> TotaisSolicitacoes { get; set; }
        public List<ExtratoTotalModel> TotaisMovimentos { get; set; }
        public List<DashSolicitacoesModel> TotalSolicitacoesStatus { get; set; }
        public List<SolicitacaoDto> UltimasSolicitacoes { get; set; }

        public decimal TotalSolicitado { get => this.TotalSolicitacoesStatus?.Sum(y=>y.Total)??0; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal TotalSaldos { get => TotalCreditos + TotalDebitos; }
        public decimal TotalPendente { get; set; }
        public decimal TotalDisponivel { get => TotalSaldos - TotalPendente; }

        public List<dynamic> ChartData { get; set; }
    }


    public class DashSolicitacoesModel
    {
        public ESolicitacaoStatus Status { get; set; }
        public decimal Total { get; set; }
    }
}
