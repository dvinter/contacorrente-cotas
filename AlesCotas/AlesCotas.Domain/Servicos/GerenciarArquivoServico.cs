using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class GerenciarArquivoServico : Servico, IGerenciarArquivoServico
    {
        public GerenciarArquivoServico(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public string Upload()
        {
            throw new NotImplementedException();
        }

        public string Upload(Solicitacao solicitacao)
        {
            return "";
        }

        public bool DeletarArquivo()
        {
            throw new NotImplementedException();
        }
    }
}
