using Application.DTOs;
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
    public virtual DbSet<AreaEntity> Area { get; set; }
    public virtual DbSet<CiudadEntity> Ciudad { get; set; }
    public virtual DbSet<DepartamentoEntity> Departamento { get; set; }
    public virtual DbSet<EmpleadoEntity> Empleado { get; set; }
    public virtual DbSet<FacultadEntity> Facultad { get; set; }
    public virtual DbSet<PaisEntity> Pais { get; set; }
    public virtual DbSet<ProgramaEntity> Programa { get; set; }
    public virtual DbSet<TipoEmpleadoEntity> TipoEmpleado { get; set; }
    public virtual DbSet<SedeEntity> Sede { get; set; }
    public virtual DbSet<TipoContratoEntity> TipoContrato { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<AreaEntity>()
                .Property(e => e.Codigo);

        modelBuilder.Entity<CiudadEntity>()
                .Property(e => e.Codigo);

        modelBuilder.Entity<DepartamentoEntity>()
                .Property(e => e.Codigo);

        modelBuilder.Entity<EmpleadoEntity>();

        modelBuilder.Entity<FacultadEntity>()
                .Property(e => e.Codigo);

        modelBuilder.Entity<PaisEntity>()
                .Property(e => e.Codigo);

        modelBuilder.Entity<ProgramaEntity>()
                .Property(e => e.Codigo);
        
        modelBuilder.Entity<TipoEmpleadoEntity>();
        
        modelBuilder.Entity<SedeEntity>()
                .Property(e => e.Codigo);
        
        modelBuilder.Entity<TipoContratoDTO>();


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
