using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace AlesCotas.Infra.Data.Repositories
{
    public class ProdutoTipoRepository : Repository<ProdutoTipo>, IProdutoTipoRepository
    {
        public ProdutoTipoRepository(ApplicationDbContext context, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {

        }

        public override IQueryable<ProdutoTipo> QueryAll(Expression<Func<ProdutoTipo, bool>> predicate = null)
        {
            return base.QueryAll(predicate).Include(x=>x.Gestores).Include(x=>x.Usuarios).Include(x=>x.Campos).Include(x=>x.Gestores);
        }

        public override async Task<ProdutoTipo> Update(ProdutoTipo entity, IDbContextTransaction? transacao = null)
        {
            var db = await QueryAll(x => x.Id.Equals(entity.Id)).FirstOrDefaultAsync();

            if (db == null)
                throw new Exception("Registro inválido");

            Context.Entry(db).CurrentValues.SetValues(entity);

            db.Usuarios.Clear();
            db.Campos.Clear();
            db.Gestores.Clear();

            foreach (var up in entity.Usuarios)
                db.Usuarios.Add(up);
            foreach (var cp in entity.Campos)
                db.Campos.Add(cp);

            foreach (var gt in entity.GestoresId)
                db.Gestores.Add(new ProdutoTipoGestor { ProdutoTipoId = entity.Id, UsuarioId = gt });

            return await base.Update(db, transacao);
        }
    }
}
