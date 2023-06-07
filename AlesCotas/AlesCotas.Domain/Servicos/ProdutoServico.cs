using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class ProdutoServico : Servico, IProdutoServico
    {
        private readonly ISolicitacaoItemServico _solicitacaoItemServ;

        public ProdutoServico(IUnitOfWork unitOfWork, ISolicitacaoItemServico solicitacaoItemServ) : base(unitOfWork)
        {
            _solicitacaoItemServ = solicitacaoItemServ;
        }

        public List<Produto> Listar(ProdutoFiltro filtro)
        {
            return _unitOfWork.ProdutoRepository.Listar(filtro).ToList();
        }

        public IQueryable<Produto>? Listar(int[] ids)
        {
            return _unitOfWork.ProdutoRepository.ListarPorIds(ids);
        }

        public Produto? Recuperar(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                throw new Exception("Digite um Id Válido");
            }

            return _unitOfWork.ProdutoRepository.Recuperar(id);
        }

        public void Cadastrar(Produto produto)
        {
            _unitOfWork.ProdutoRepository.Cadastrar(produto);
        }

        public void Atualizar(Produto produto)
        {
            _unitOfWork.ProdutoRepository.Atualizar(produto);
        }

        public bool Deletar(int id)
        {
            var entity = Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um tipo com esse ID!");
            }

            // Checar se existe algum registro com o produto em questão
            if (_solicitacaoItemServ.Listar().Where(item => item.IDProduto.Equals(id)).ToList().Any())
            {
                entity.Desativar();
                _unitOfWork.ProdutoRepository.Atualizar(entity);
                return true;
            }
            else
            {
                _unitOfWork.ProdutoRepository.Deletar(entity);
                return true;
            }

        }
    }
}
