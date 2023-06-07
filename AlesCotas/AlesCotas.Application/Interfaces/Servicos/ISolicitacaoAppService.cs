using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Application.Models;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface ISolicitacaoAppService : IAppServico<Solicitacao, SolicitacaoDto, SolicitacaoFilter>
    {
        Task<bool> NovaSolicitacao(NovaSolicitacaoModel model, ProdutoTipoDto produtoTipo);
        Task<bool> SetStatusSolicitacao(int id, ESolicitacaoStatus status);
        Task<SolicitacaoAnexos> GetAnexo(int anexoId);

        Task<bool> ProcessarAprovacaoAutomatica();
    }
}
