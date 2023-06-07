using AlesCotas.Application.Classes;
using AlesCotas.Application.Filter.Base;
using AlesCotas.Application.Interfaces.Servicos.Base;
using AlesCotas.Domain.Entities;
using AlesCotas.Domain.Interfaces.Base;
using AlesCotas.Domain.Interfaces.Repositories.Base;
using AlesCotas.Infra.Data.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AlesCotas.Application.Servicos.Base
{
    /// <summary>
    /// Classe abstrata que define a camada de serviço da aplicação.
    /// Implementa métodos de Criar, Atualizar, Apagar e Listar os registros de acordo com os Tipos de entidades informados na implementação
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade que o Serviço irá trabalhar</typeparam>
    /// <typeparam name="TDto">Tipo de DTO que o Serviço irá trabalhar</typeparam>
    /// <typeparam name="TFilter">Tipo de Filtro que o serviço irá trabalhar</typeparam>
    public abstract class AppServico<TEntity, TDto, TFilter> : IAppServico<TEntity, TDto, TFilter> where TEntity : class, IBaseEntity where TDto : class where TFilter : IBaseFilter 
    {
        protected readonly IMapper _mapper;
        protected readonly IRepository<TEntity> _repository;
        protected readonly ICurrentUserManager _currentUserManager;
        public AppServico(IMapper mapper, IRepository<TEntity> repository, ICurrentUserManager currentUserManager)
        {
            _currentUserManager = currentUserManager;
            _mapper = mapper;
            _repository = repository;
        }

        /// <summary>
        /// Busca o registro pelo ID informado, e chama o método de Delete do repositório
        /// Se necessário alguma regra de negócio antes da deleção, sobrescrever este método
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Apagar(int id)
        {
            var obj = await _repository.QueryAll().FirstOrDefaultAsync(m => m.Id == id);
            if (obj != null)
            {
                var entry = await _repository.Delete(obj);
                await _repository.Commit();
            }
            return obj;
        }

        /// <summary>
        /// Chama o método Update do repositório.
        /// Se necessário alguma regra de negócio antes da atualização, sobrescrever este método
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Atualizar(TEntity entity)
        {
            
            var entry = await _repository.Update(entity);
            await _repository.Commit();
            return entry;
        }

        /// <summary>
        /// Chama o método Insert do repositório.
        /// Se necessário alguma regra de negócio antes da inserção, sobrescrever este método
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Cadastrar(TEntity entity)
        {
            var entry = await _repository.Insert(entity);
            await _repository.Commit();
            return entry;
        }

        /// <summary>
        /// Conta quantos registros atendem ao predicado informado
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.QueryAll(predicate).CountAsync();               
        }
        /// <summary>
        /// Método abstrato que deve ser implementado na classe filha para select do registro como um DTO
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public abstract IQueryable<TDto> QueryDto(Expression<Func<TEntity, bool>> predicate = null);

        /// <summary>
        /// Método abstrato que deve ser implementado na classe filha para listar os registros como DTO, filtrando pelo Tipo de filtro específico
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public abstract Task<ListaPaginada<TDto>> ListPag(TFilter filter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<List<TDto>> ListDto(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.QueryAll(predicate)
                
                .Select(x => _mapper.Map<TDto>(x)).ToListAsync();
        }

        /// <summary>
        /// Retorna o primeiro registro que atende ao predicado informado
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> Select(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.QueryAll(predicate).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retorna o primeiro registro, através de um DTO, que atende ao predicado informado
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<TDto> SelectDto(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await QueryDto(predicate).FirstOrDefaultAsync();
        }

        
        /// <summary>
        /// Executa a Query informada, fazendo a paginação
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paginaAtual"></param>
        /// <param name="tamanhoPagina"></param>
        /// <returns></returns>
        protected async Task<ListaPaginada<TDto>> ToPagedList(IQueryable<TDto> source, int paginaAtual, int tamanhoPagina)
        {
            var totalRegistros = await source.CountAsync();
            var items = await source.Skip((paginaAtual - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();
            return new ListaPaginada<TDto>(items, totalRegistros, paginaAtual, tamanhoPagina);
        }

        /// <summary>
        /// Executa um select completo no repositório, filtrando pelo predicado informado
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> List(Expression<Func<TEntity, bool>> predicate = null)
        {
            return await _repository.QueryAll(predicate).ToListAsync();
        }
    }
}
