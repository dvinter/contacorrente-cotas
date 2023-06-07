using AlesCotas.Domain.Constantes.Solicitacao;
using AlesCotas.Domain.Entities_old;
using AlesCotas.Domain.Filtros;
using AlesCotas.Domain.Interfaces.Servicos;
using AlesCotas.Domain.Interfaces.UnitOfWork;
using AlesCotas.Domain.Servicos.Base;

namespace AlesCotas.Domain.Servicos
{
    public class SolicitacaoServico : Servico, ISolicitacaoServico
    {
        private readonly IProdutoServico _produtoServico;

        public SolicitacaoServico(IUnitOfWork unitOfWork, IProdutoServico produtoServico) : base(unitOfWork)
        {
            _produtoServico = produtoServico;
        }

        public IQueryable<Solicitacao> Listar(SolicitacaoFiltro filtro)
        {
            return _unitOfWork.SolicitacaoRepository.Listar(filtro);
        }

        public Solicitacao? Recuperar(int id)
        {
            return _unitOfWork.SolicitacaoRepository.Recuperar(id);
        }

        public void Cadastrar(Solicitacao solicitacao)
        {
            ValidarSolicitacao(solicitacao, false);
            _unitOfWork.SolicitacaoRepository.Cadastrar(solicitacao);
        }

        public void Atualizar(Solicitacao solicitacao)
        {
            ValidarSolicitacao(solicitacao, true);
            _unitOfWork.SolicitacaoRepository.Atualizar(solicitacao);
        }

        public bool Deletar(int id)
        {
            var entity = Recuperar(id);

            if (entity == null)
            {
                throw new Exception("Não existe um tipo com esse ID!");
            }

            _unitOfWork.SolicitacaoRepository.Deletar(entity);
            return true;
        }

        public void Aprovar(Solicitacao solicitacao)
        {

            if (solicitacao.Status != SolicitacaoStatus.Pendente)
            {
                throw new Exception($"Apenas solicitações com status '{SolicitacaoStatus.Pendente}' podem ser aprovadas");
            }

            // Se o valor aprovado nao foi informado, automaticamente é setado o valor solicitado.
            if (solicitacao.ValorAprovado == decimal.Zero)
            {
                solicitacao.ValorAprovado = solicitacao.ValorSolicitado;
            }

            if (solicitacao.ValorSolicitado != solicitacao.ValorAprovado)
            {
                solicitacao.AprovacaoParcial();
            }
            else
            {
                solicitacao.Aprovar();
            }

            if (solicitacao.Tipo == SolicitacaoTipos.Combustivel)
            {
                solicitacao.Items.First().QuantidadeAutorizada = solicitacao.Items.First().QuantidadeSolicitada;
            }

            _unitOfWork.SolicitacaoRepository.Atualizar(solicitacao);
        }

        public void Negar(Solicitacao solicitacao)
        {

            if (solicitacao.Status != SolicitacaoStatus.Pendente)
            {
                throw new Exception($"Apenas solicitações com status '{SolicitacaoStatus.Pendente}' podem ser Negadas");
            }

            solicitacao.ValorAprovado = decimal.Zero;

            solicitacao.Negar();
            _unitOfWork.SolicitacaoRepository.Atualizar(solicitacao);
        }

        private void ValidarSolicitacao(Solicitacao solicitacao, bool isUpdate)
        {
            if (solicitacao.Items == null || solicitacao.Items.Count == 0)
            {
                throw new Exception("Pelo menos 1 item precisa ser informado");
            }

            if (string.IsNullOrEmpty(solicitacao.Tipo))
            {
                throw new Exception("Solicitação precisa ter um tipo");
            }

            var produtos = _produtoServico.Listar(solicitacao.Items.Select(e => e.IDProduto).ToArray());

            if (produtos == null || !produtos.Any())
            {
                throw new Exception("Não existem produtos com os Ids informados");
            }

            if (!isUpdate)
            {
                if (produtos.Where(e => e.Tipo.Nome != solicitacao.Tipo).Any())
                {
                    throw new Exception("Todos os produtos devem ter o mesmo tipo informado na solicitação");
                }
            }

            solicitacao.Justificativa = "Sem justificativa";

            solicitacao.ValorSolicitado = CalcularTotal(solicitacao.Tipo, solicitacao.Items, produtos.ToList());

            if (solicitacao.Tipo != SolicitacaoTipos.Combustivel)
            {
                solicitacao.Items = AtualizarValorUnitarioItems(solicitacao.Items, produtos.ToList());
            }
        }

        private decimal CalcularTotal(string tipo, ICollection<SolicitacaoItem> items, List<Produto> produtos)
        {

            switch (tipo)
            {
                case SolicitacaoTipos.Combustivel:
                    return items.First().ValorUnitario;

                case SolicitacaoTipos.CopiaReprografica:
                    {
                        decimal total = 0;

                        foreach (var item in items)
                        {
                            decimal valorUnitario = produtos.Find(p => p.IDProduto == item.IDProduto).ValorUnitario;
                            total += decimal.Multiply(valorUnitario, item.QuantidadeSolicitada);
                        }

                        return total;
                    }

                case SolicitacaoTipos.MaterialExpediente:
                    {
                        decimal total = 0;

                        foreach (var item in items)
                        {
                            decimal valorUnitario = produtos.Find(p => p.IDProduto == item.IDProduto).ValorUnitario;
                            total += decimal.Multiply(valorUnitario, item.QuantidadeSolicitada);
                        }

                        return total;
                    }

                default:
                    throw new Exception("Tipo inválido!");
            }
        }

        private ICollection<SolicitacaoItem> AtualizarValorUnitarioItems(ICollection<SolicitacaoItem> items, List<Produto> produtos)
        {

            foreach (var item in items)
            {
                Produto? produto = produtos.Find(p => p.IDProduto == item.IDProduto);

                if (produto == null)
                {
                    throw new Exception("Produto informado nao existe");
                }

                item.ValorUnitario = produto.ValorUnitario;
            }

            return items;
        }
    }
}
