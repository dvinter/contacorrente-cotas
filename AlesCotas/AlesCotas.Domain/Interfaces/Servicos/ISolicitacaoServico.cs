using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos.Base;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface ISolicitacaoServico : IServico
    {
        public IQueryable<Solicitacao> Listar(SolicitacaoFiltro filtro);
        public Solicitacao? Recuperar(int id);
        public void Cadastrar(Solicitacao solicitacao);
        public void Atualizar(Solicitacao solicitacao);
        public bool Deletar(int id);
        public void Aprovar(Solicitacao solicitacao);
        public void Negar(Solicitacao solicitacao);
    }
}
