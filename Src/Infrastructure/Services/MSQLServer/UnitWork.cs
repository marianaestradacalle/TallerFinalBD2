using Microsoft.EntityFrameworkCore.Storage;

namespace Services.MSQLServer;
public class UnitWork: IDisposable
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
        ContextTransaction?.Commit();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
        ContextTransaction?.Dispose();
    }

    public void RollBack()
    {

        ContextTransaction?.Rollback();
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
