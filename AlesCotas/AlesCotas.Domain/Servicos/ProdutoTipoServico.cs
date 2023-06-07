using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class ProdutoTipoServico : Servico, IProdutoTipoServico
    {
        public ProdutoTipoServico(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<ProdutoTipo> Listar(ProdutoTipoFiltro filtro)
        {
            return _unitOfWork.ProdutoTipoRepository.Listar(filtro);
        }

        public ProdutoTipo? Recuperar(int id)
        {
            return _unitOfWork.ProdutoTipoRepository.Recuperar(id);
        }

        public void Cadastrar(ProdutoTipo entity)
        {
            _unitOfWork.ProdutoTipoRepository.Cadastrar(entity);
        }

        public void Atualizar(ProdutoTipo entity)
        {
            _unitOfWork.ProdutoTipoRepository.Atualizar(entity);
        }

        public void Deletar(int id)
        {
            var entity = _unitOfWork.ProdutoTipoRepository.Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um tipo com esse ID!");
            }

            if (TemRelacionamento(entity))
            {
                entity.Inativar();
                _unitOfWork.ProdutoTipoRepository.Atualizar(entity);
                return;
            }

            _unitOfWork.ProdutoTipoRepository.Deletar(entity);
        }

        private bool TemRelacionamento(ProdutoTipo produtoTipo)
        {

            var produtos = _unitOfWork.ProdutoRepository.Listar().Where(produto => produto.IDProdutoTipo == produtoTipo.IDProdutoTipo).ToList();

            if (produtos.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
