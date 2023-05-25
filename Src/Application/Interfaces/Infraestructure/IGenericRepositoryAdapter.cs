using System.Linq.Expressions;

namespace Application.Interfaces.Infraestructure;
public interface IGenericRepositoryAdapter<TEntity> : IDisposable where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool track = false);
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool track = false);
    Task<TEntity> FindByIdAsync(object key);
    Task<TEntity> FindByIdAsync(params object[] keys);
    TEntity FindById(object key);
    TEntity FindById(params object[] keys);
    Task AddAsync(TEntity entity);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void Delete(object key);
    void Delete(params object[] keys);
}
