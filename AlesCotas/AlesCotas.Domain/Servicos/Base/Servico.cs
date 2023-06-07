using AlesCotas.Domain.Interfaces.Servicos.Base;
using AlesCotas.Domain.Interfaces.UnitOfWork;

namespace AlesCotas.Domain.Servicos.Base
{
    public class Servico : IServico
    {
        protected readonly IUnitOfWork _unitOfWork;

        public Servico(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
