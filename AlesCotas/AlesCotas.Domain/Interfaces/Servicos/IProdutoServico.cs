using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos.Base;

namespace AlesCotas.Domain.Interfaces.Servicos
{
    public interface IProdutoServico : IServico
    {
        public List<Produto> Listar(ProdutoFiltro filtro);
        public IQueryable<Produto>? Listar(int[] ids);
        public Produto? Recuperar(int id);
        public void Cadastrar(Produto produto);
        public void Atualizar(Produto produto);
        public bool Deletar(int id);
    }
}
