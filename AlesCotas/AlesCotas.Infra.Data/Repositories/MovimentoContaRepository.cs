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
  

    public class MovimentoContaRepository : Repository<MovimentoConta>, IMovimentoContaRepository
    {
        private readonly IFluxoContaRepository _fluxoContaRepository;
        public MovimentoContaRepository(ApplicationDbContext context,IFluxoContaRepository fluxoContaRepository, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {
            _fluxoContaRepository = fluxoContaRepository;
        }

      
    }
}
