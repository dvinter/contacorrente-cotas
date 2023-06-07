using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Enum;
using AlesCotas.Domain.Interfaces.Repositories;
using AlesCotas.Infra.CrossCutting.Utils.Extensions;
using AlesCotas.Infra.Data.Context;
using AlesCotas.Infra.Data.Identity;
using AlesCotas.Infra.Data.Repositories.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;

namespace AlesCotas.Infra.Data.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioRepository(ApplicationDbContext context, UserManager<Usuario> userManager, ICurrentUserManager currentUserManager) : base(context, currentUserManager)
        {
            _userManager = userManager;
        }

        public override IQueryable<Usuario> QueryAll(Expression<Func<Usuario, bool>> predicate = null)
        {
            var query = _userManager.Users.Include(e => e.Gabinete).Where(x=>!x.UserName.Equals("system")) .AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query.OrderBy(e => e.Nome);
        }

        public override async Task<Usuario> Insert(Usuario entity, IDbContextTransaction? transaction = null)
        {
            var user = await this._currentUserManager.GetCurrentUser();
            var entry = await _userManager.CreateAsync(entity);

            if (entry.Succeeded)
            {


                this.Context.Logs.Add(new Log() { EventType = Domain.Enum.ETipoEventoLog.Insert, RefId = entity.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Inserção de usuário: " + entity.Matricula + "/" + entity.Role.GetEnumDescription(), EventDate = DateTime.Now });
                await this.AddRole(entity, entity.Role.Value);
                await this.Context.SaveChangesAsync();
                return entity;
            }
            else
                throw new Exception(entry.Errors.FirstOrDefault().Description);

        }

        public override async Task<Usuario> Update(Usuario entity, IDbContextTransaction? transacao = null)
        {
            var user = await this._currentUserManager.GetCurrentUser();

            var dbUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (dbUser != null)
            {
                dbUser.Email = entity.Email;
                dbUser.Setor = entity.Setor;
                dbUser.Ativo = entity.Ativo;
                dbUser.GabineteId = entity.GabineteId;
                var result = await _userManager.UpdateAsync(dbUser);

                if (result.Succeeded)
                {
                    this.Context.Logs.Add(new Log() { EventType = Domain.Enum.ETipoEventoLog.Update, RefId = dbUser.Id, Entity = GetTableName(), UserId = user.Id, IpAddress = this._currentUserManager.GetRemoteAddress(), Description = "Alteração de usuário: " + entity.Matricula + "/" + entity.Role.GetEnumDescription(), EventDate= DateTime.Now });
                    var roles = (await this._userManager.GetRolesAsync(dbUser)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>();

                    if (!roles.Any(x => x.Equals(entity.Role)))
                    {
                        await this.AddRole(dbUser, entity.Role.Value);
                    }
                    foreach (var role in roles)
                    {
                        if (role != entity.Role)
                        {
                            await this.RemoveRole(dbUser, role);
                        }
                    }
                }


                return dbUser;
            }
            return entity;
        }

        
        public async Task<IdentityResult> Deletar(Usuario entity)
        {
            var entry = await _userManager.DeleteAsync(entity);
            return entry;
        }

        public async Task<IEnumerable<EUsuarioPerfil>> GetRoles(Usuario entity)
        {
            var roles = (await this._userManager.GetRolesAsync(entity)).Select(x => Enum.Parse(typeof(EUsuarioPerfil), x)).OrderBy(x => x).Cast<EUsuarioPerfil>();
            return roles;

        }

        public IQueryable<UsuarioRoles> QueryRoles()
        {
            return this.Context.UserRoles.Join(this.Context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => new UsuarioRoles
            {
                 UsuarioId  = ur.UserId,
                  RoleId = ur.RoleId,
                   Perfil = r.Perfil
            });
        }

        public async Task<IdentityResult> AddRole(Usuario entity, EUsuarioPerfil perfil)
        {
            return await this._userManager.AddToRoleAsync(entity, ((int)perfil).ToString());
        }

        public async Task<IdentityResult> RemoveRole(Usuario entity, EUsuarioPerfil perfil)
        {
            return await this._userManager.RemoveFromRoleAsync(entity, ((int)perfil).ToString());
        }

    }
}
