using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Interfaces.Servicos.Base;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface ISolicitacaoItemServico : IServico
    {
        public IQueryable<SolicitacaoItem> Listar();
        public void Cadastrar(SolicitacaoItem item);
        public void Atualizar(SolicitacaoItem item);
        public SolicitacaoItem? Recuperar(int id);
        public bool Deletar(int id);
    }
}
