using Application.Interfaces.Infraestructure;
using AutoMapper;
using Core.Entities;
using Dobles.Tests.Core.Tests.Entities;
using Dobles.Tests.Fake.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dobles.Tests.Fake.InfraestructureServices;
public class FakeGenericRepositoryServiceNotes<TEntity> : IGenericRepositoryAdapter<TEntity> where TEntity : class
{
    private readonly IMapper _mapper;
    private readonly FakeMapper _fakeMapper = new FakeMapper();

    public List<Notes> notes { get; set; }
    public Notes notesBuild { get; set; }
    public FakeGenericRepositoryServiceNotes()
    {
        notes = new List<Notes>();
        notesBuild = new NotesBuilder().Notes();
        notes.Add(notesBuild);

        _mapper = (IMapper)_fakeMapper.GetFakeMapper();
    }
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
    IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool track = false)
    {
        return _mapper.Map<IEnumerable<TEntity>>(filter);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
    IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "", bool track = false)
    {
        return await Task.Run(() =>
        {

            return _mapper.Map<IEnumerable<TEntity>>(filter);
        });
    }

    public async Task<TEntity> FindByIdAsync(object key)
    {
        return await Task.Run(() =>
        {
            var query = notes.Where(x => x.Id == key.ToString()).FirstOrDefault();
            return _mapper.Map<TEntity>(query);
        });
    }

    public async Task<TEntity> FindByIdAsync(params object[] keys)
    {
        return await Task.Run(() =>
        {
            var query = notes.Where(x => x.Id == keys.ToString()).FirstOrDefault();
            return _mapper.Map<TEntity>(query);
        });
    }

    public async Task AddAsync(TEntity entity)
    {
        await Task.Run(() =>
        {
            TEntity note = entity;
        });
    }

    public void Update(TEntity entity)
    {
        TEntity note = entity;
    }

    public void Delete(TEntity entity)
    {
        Console.WriteLine("Stub Delete respository");
    }

    public TEntity FindById(object key)
    {
        var query = notes.Where(x => x.Id == key.ToString()).FirstOrDefault();
        return _mapper.Map<TEntity>(query);
    }

    public TEntity FindById(params object[] keys)
    {
        var query = notes.Where(x => x.Id == keys.ToString()).FirstOrDefault();
        return _mapper.Map<TEntity>(query);
    }

    public void Add(TEntity entity)
    {
        Console.WriteLine("Stub Add respository");
    }

    public void Delete(object key)
    {
        Console.WriteLine("Stub Delete respository");
    }

    public void Delete(params object[] keys)
    {
        Console.WriteLine("Stub Delete respository");
    }

    public void Dispose()
    {
        Console.WriteLine("Stub Dispose respository");
    }
}
