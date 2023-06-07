using AlesCotas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.Data.Identity
{
    public interface ICurrentUserManager
    {
        public Task<Usuario> GetCurrentUser();
        public Task<Usuario> GetUser(string name);

        public string GetRemoteAddress();
    }
}
