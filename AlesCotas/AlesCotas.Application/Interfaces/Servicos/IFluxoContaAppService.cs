using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface IFluxoContaAppService
    {
        Task<int> ProcessarFila();
        Task<ExtratoViewModel> SelectExtrato(ExtratoFilter filter);
    }
}
