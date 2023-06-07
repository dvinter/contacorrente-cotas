using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos.Base;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface ISolicitacaoItemArquivoServico : IServico
    {
        public void Cadastrar(SolicitacaoItemArquivo entity);
        public void Atualizar(SolicitacaoItemArquivo entity);
        public SolicitacaoItemArquivo? Recuperar(int id);
    }
}
