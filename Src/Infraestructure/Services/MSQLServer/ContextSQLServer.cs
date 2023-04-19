using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Services.MSQLServer;
public partial class ContextSQLServer : DbContext
{
    public ContextSQLServer()
    {
    }
    public ContextSQLServer(DbContextOptions<ContextSQLServer> options) : base(options)
    {

    }
    public virtual DbSet<Notes> Notes { get; set; }
    public virtual DbSet<NoteLists> NoteLists { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {          
       OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
