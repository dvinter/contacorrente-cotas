using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class SolicitacaoItemArquivoServico : Servico, ISolicitacaoItemArquivoServico
    {
        public SolicitacaoItemArquivoServico(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public void Cadastrar(SolicitacaoItemArquivo entity)
        {
            _unitOfWork.SolicitacaoItemArquivoRepository.Cadastrar(entity);
        }

        public void Atualizar(SolicitacaoItemArquivo entity)
        {
            _unitOfWork.SolicitacaoItemArquivoRepository.Atualizar(entity);
        }

        public SolicitacaoItemArquivo? Recuperar(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                throw new Exception("Digite um Id Válido");
            }

            return _unitOfWork.SolicitacaoItemArquivoRepository.Recuperar(id);
        }
    }
}
