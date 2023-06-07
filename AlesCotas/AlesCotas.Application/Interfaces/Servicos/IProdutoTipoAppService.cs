using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Domain.Entities;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface IProdutoTipoAppService : IAppServico<ProdutoTipo, ProdutoTipoDto, ProdutoTipoFilter>
    {
    }
}
