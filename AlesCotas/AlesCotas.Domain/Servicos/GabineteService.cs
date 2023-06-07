using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlesCotas.Domain.Servicos
{
    

    public class GabineteService : Servico, IGabineteService
    {
        public GabineteService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<Gabinete> Listar(GabineteFiltro filtro)
        {
            return _unitOfWork.GabineteRepository.Listar(filtro);
        }

        public async Task<Gabinete> Cadastrar(Gabinete item)
        {
            return await _unitOfWork.GabineteRepository.Cadastrar(item);
        }

        public Gabinete Atualizar(Gabinete item)
        {
            return _unitOfWork.GabineteRepository.Atualizar(item);
        }

        public async Task<Gabinete?> Recuperar(int id)
        {
            return await _unitOfWork.GabineteRepository.Recuperar(id);
        }

        public async Task<bool> Deletar(int id)
        {
            var entity = await Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um gabinete com esse ID!");
            }

            _unitOfWork.GabineteRepository.Deletar(entity);
            return true;
        }

    }
}
