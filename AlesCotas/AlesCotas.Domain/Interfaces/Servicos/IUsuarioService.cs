using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface IUsuarioServico
    {
        Task<IdentityResult> Atualizar(Usuario item);
        Task<IdentityResult> Cadastrar(Usuario item);

        Task<IdentityResult> SetRole(Usuario item, string Role);
        Task<string?> GetRole(Usuario item);
        IQueryable<UsuarioRoles> ListarRoles();
        Task<IdentityResult> Deletar(string id);
        IQueryable<Usuario> Listar(UsuarioFiltro filtro);
        Task<Usuario?> Recuperar(string id);
    }
}
