using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Infra.Data.Repositories
{
    public class SolicitacaoAdiantamentoCotaRepository : Repository<SolicitacaoAdiantamentoCota>, ISolicitacaoAdiantamentoCotaRepository
    {
        public SolicitacaoAdiantamentoCotaRepository(ApplicationDbContext context, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {

        }
    }
}
