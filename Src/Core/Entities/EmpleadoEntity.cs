namespace Core.Entities;
public class EmpleadoEntity
{
    public string? Identificacion { get; set; }
    public string? Nombres { get; set; }
    public string? Apellidos { get; set; }
    public string? Email { get; set; }
    public string? TipoContratacion { get; set; }
    public string? TipoEmpleado { get; set; }
    public int CodFacultad { get; set; }
    public int CodSede { get; set; }
    public int LugarNacimiento { get; set; }
}
