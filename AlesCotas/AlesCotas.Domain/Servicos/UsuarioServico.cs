using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;
using Microsoft.AspNetCore.Identity;


namespace AlesCotas.Domain.Servicos
{
    public class UsuarioServico : Servico, IUsuarioServico
    {
      
        public UsuarioServico(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
          
        }

        public IQueryable<Usuario> Listar(UsuarioFiltro filtro)
        {
            
            return _unitOfWork.UsuarioRepository.Listar(filtro);
        }
        public IQueryable<UsuarioRoles> ListarRoles()
        {

            return _unitOfWork.UsuarioRepository.QueryRoles();
        }


        public async Task<IdentityResult> Cadastrar(Usuario user)
        {
            var result =  await _unitOfWork.UsuarioRepository.Cadastrar(user);
            return result;
        }

        public async Task<IdentityResult> Atualizar(Usuario item)
        {
            return await  _unitOfWork.UsuarioRepository.Atualizar(item);
        }

        public async Task<Usuario?> Recuperar(string id)
        {
            return await  _unitOfWork.UsuarioRepository.Recuperar(id);
        }

        public async Task<IdentityResult> Deletar(string id)
        {
            var entity = await Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um usuário com esse ID!");
            }

            var result = await _unitOfWork.UsuarioRepository.Deletar(entity);
            return result;
        }

        public async Task<IdentityResult> SetRole(Usuario item, string newRole)
        {
            var currentRoles = await _unitOfWork.UsuarioRepository.GetRoles(item);
            foreach (var role in currentRoles)
                await _unitOfWork.UsuarioRepository.RemoveRole(item, role);

            return await _unitOfWork.UsuarioRepository.AddRole(item, newRole);
        }
        public async Task<string?> GetRole(Usuario item)
        {
            var currentRoles = await _unitOfWork.UsuarioRepository.GetRoles(item);
            return currentRoles?.FirstOrDefault();
        }
    }
}
