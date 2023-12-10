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
    public virtual DbSet<SedeEntity> Sede { get; set; }
    public virtual DbSet<CiudadEntity> Ciudad { get; set; }
    public virtual DbSet<AsistenteEntity> Asistente { get; set; }
    public virtual DbSet<ComentarioEntity> Comentario { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {          
       OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
