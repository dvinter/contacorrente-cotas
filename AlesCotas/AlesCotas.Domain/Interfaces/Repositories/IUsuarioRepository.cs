using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        public IQueryable<UsuarioRoles> QueryRoles();
        Task<IEnumerable<EUsuarioPerfil>> GetRoles(Usuario entity);
    }
}
