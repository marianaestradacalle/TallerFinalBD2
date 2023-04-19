using Application.Interfaces.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Services.MSQLServer;
public class GenericRepositoryService<TEntity> : IGenericRepositoryService<TEntity> where TEntity : class
{
    private ContextSQLServer context;
    private DbSet<TEntity> dbSet;

    public GenericRepositoryService(ContextSQLServer context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
             IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool track = false)
    {

        IQueryable<TEntity> query = (!track) ? dbSet.AsNoTracking() : dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return orderBy(query).ToList();
        }
        else
        {
            return query.ToList();
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool track = false)
    {

        IQueryable<TEntity> query = (!track) ? dbSet.AsNoTracking() : dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
            (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        return (orderBy != null) ? await orderBy(query).ToListAsync().ConfigureAwait(false) : await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<TEntity> FindByIdAsync(object key)
    {
        return await dbSet.FindAsync(key);
    }

    public async Task<TEntity> FindByIdAsync(params object[] keys)
    {
        return await dbSet.FindAsync(keys);
    }

    public async Task AddAsync(TEntity entity)
    {
        if (entity != null)
        {
            await dbSet.AddAsync(entity);
        }
    }

    public void Update(TEntity entity)
    {
        if (entity != null)
        {
            dbSet.Update(entity);
        }
    }

    public void Delete(TEntity entity)
    {
        if (entity != null)
        {
            dbSet.Remove(entity);
        }
    }

    public TEntity FindById(object key)
    {
        return dbSet.Find(key);
    }

    public TEntity FindById(params object[] keys)
    {
        return dbSet.Find(keys);
    }

    public void Add(TEntity entity)
    {
        if (entity != null)
        {
            dbSet.Add(entity);
        }
    }

    public void Delete(object key)
    {
        var item = FindById(key);
        if (item != null)
        {
            Delete(item);
        }
    }

    public void Delete(params object[] keys)
    {
        var item = FindById(keys);
        if (item != null)
        {
            Delete(item);
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
