using AlesCotas.Application.Dto;
using AlesCotas.Application.Filter;
using AlesCotas.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Application.Interfaces.Servicos
{
    public interface IDashBoardAppService
    {
        Task<IEnumerable<GabineteDto>> GetGabinetes();
        Task<IEnumerable<ProdutoTipoDto>> GetProdutoTipos();

        Task<DashBoardViewModel> GetDashBoard(DashBoardFilter filtro);

    }
}
