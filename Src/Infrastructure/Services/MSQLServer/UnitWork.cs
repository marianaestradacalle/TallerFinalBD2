using Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Services.MSQLServer;
public class UnitWork: IUnitWork, IDisposable
{
    private readonly ContextSQLServer _context;
    private IDbContextTransaction ContextTransaction { get; set; }


    public UnitWork(ContextSQLServer context)
    {
        _context = context;
    }

    public void CreateTransaction()
    {
        ContextTransaction = _context.Database.BeginTransaction();
    }
    public void Commit()
    {
        if (ContextTransaction != null)
        {
            ContextTransaction.Commit();
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        if (ContextTransaction != null)
        {
            ContextTransaction.Dispose();
        }
    }

    public void RollBack()
    {

        if (ContextTransaction != null)
        {
            ContextTransaction.Rollback();
        }
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
