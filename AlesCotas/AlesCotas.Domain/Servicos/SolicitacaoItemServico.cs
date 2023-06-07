using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class SolicitacaoItemServico : Servico, ISolicitacaoItemServico
    {
        public SolicitacaoItemServico(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<SolicitacaoItem> Listar()
        {
            return _unitOfWork.SolicitacaoItemRepository.Listar();
        }

        public void Cadastrar(SolicitacaoItem item)
        {
            _unitOfWork.SolicitacaoItemRepository.Cadastrar(item);
        }

        public void Atualizar(SolicitacaoItem item)
        {
            _unitOfWork.SolicitacaoItemRepository.Atualizar(item);
        }

        public SolicitacaoItem? Recuperar(int id)
        {
            return _unitOfWork.SolicitacaoItemRepository.Recuperar(id);
        }

        public bool Deletar(int id)
        {
            var entity = Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um tipo com esse ID!");
            }

            _unitOfWork.SolicitacaoItemRepository.Deletar(entity);
            return true;
        }
    }
}
