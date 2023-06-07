using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface IGabineteService
    {
        Gabinete Atualizar(Gabinete item);
        Task<Gabinete> Cadastrar(Gabinete item);
        Task<bool> Deletar(int id);
        IQueryable<Gabinete> Listar(GabineteFiltro filtro);
        Task<Gabinete?> Recuperar(int id);
    }
}
