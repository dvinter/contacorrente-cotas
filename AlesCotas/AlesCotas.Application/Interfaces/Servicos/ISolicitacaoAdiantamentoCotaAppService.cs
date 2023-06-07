using AlesCotas.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Application.Filter;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface ISolicitacaoAdiantamentoCotaAppService : IAppServico<SolicitacaoAdiantamentoCota,SolicitacaoAdiantamentoCotaDto, SolicitacaoAdiantamentoCotaFilter>
    {
        Task<bool> Aprovar(int id);
        Task<bool> Recusar(int id);

        Task<List<DateTime>> MesesJaRecebidos(int gabineteId);
    }
}
