using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos.Base;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface IGerenciarArquivoServico : IServico
    {
        public string Upload();
        public string Upload(Solicitacao solicitacao);
        public bool DeletarArquivo();
    }
}
