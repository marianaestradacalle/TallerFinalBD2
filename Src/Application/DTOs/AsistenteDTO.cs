using Application.Common.Helpers.Enum;

namespace Application.DTOs;
public class AsistenteDTO
{
    public string? Identificación { get; set; }
    public string? NombreUsuario { get; set; }
    public string? NombreCompleto { get; set; }
    public string? RolAsistente { get; set; }
    public string? Email { get; set; }
    public string? Ciudad { get; set; }
    public string? Celular { get; set; }
    public string? Categoria { get; set; }
}
