namespace Application.DTOs;
public class EventoDTO
{
    public string? IdEvento { get; set; }
    public string? Titulo { get; set; }
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Lugar { get; set; }
    public AsistenteDTO? InformacionAsistente { get; set; }
    public FacultadDTO? FacultadesOrganizadoras { get; set; }
    public ComentarioDTO? Comentarios { get; set; }

}
