using AlesCotas.Domain.Filtros.Enum;
using AlesCotas.Domain.Filtros;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Domain.Entities;
using AlesCotas.Infra.Data.Identity;
using System.Linq.Expressions;

namespace AlesCotas.Infra.Data.Repositories
{
  

    public class PublicacaoRepository : Repository<Publicacao>, IPublicacaoRepository
    {
        public PublicacaoRepository(ApplicationDbContext context, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {
        }


        public override IQueryable<Publicacao> QueryAll(Expression<Func<Publicacao, bool>> predicate = null)
        {
            return base.QueryAll(predicate).Include(x=>x.Gabinete);
        }
    }
}
