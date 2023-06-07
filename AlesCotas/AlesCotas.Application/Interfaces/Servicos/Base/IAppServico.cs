using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Domain.Interfaces.Base;
using System.Linq.Expressions;

namespace AlesCotas.Application.Interfaces.Servicos.Base
{
    /// <summary>
    /// Definição da base de um Serviço da aplicação
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TFilter"></typeparam>
    public interface IAppServico<TEntity, TDto, TFilter> where TEntity : class, IBaseEntity where TDto : class where TFilter : IBaseFilter
    {
        Task<ListaPaginada<TDto>> ListPag(TFilter filter);
        Task<List<TEntity>> List(Expression<Func<TEntity, bool>> predicate = null);
        IQueryable<TDto> QueryDto(Expression<Func<TEntity, bool>> predicate = null);
        Task<List<TDto>> ListDto(Expression<Func<TEntity, bool>> predicate = null);
        Task<TDto> SelectDto(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> Cadastrar(TEntity entity);
        Task<TEntity> Atualizar(TEntity entity);
        Task<TEntity> Apagar(int id);
        Task<int> Count(Expression<Func<TEntity, bool>> predicate = null);
    }
}

