using Application.Interfaces.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Dobles.Tests.Dummy.Generic;
public class DummyUnitWork : IUnitWork, IDisposable
{
    public void CreateTransaction()
    {
        Console.WriteLine("Begin transaction");
    }
    public void Commit()
    {
        Console.WriteLine("Commit transaction");
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        disposed = true;
    }

    public void Dispose()
    {
        Console.WriteLine("Dispose");
    }

    public void RollBack()
    {
        Console.WriteLine("Rollback transaction");
    }

    public void Save()
    {
        Console.WriteLine("Save changes");
    }

    public async Task SaveAsync()
    {
        await Task.Run(() =>
        {
            Console.WriteLine("Save changes async");
        });
    }
}
