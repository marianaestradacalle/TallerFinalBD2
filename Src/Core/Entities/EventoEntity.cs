namespace Core.Entities
{
    public class EventoEntity
    {
        public Guid Id { get; set; }
        public string? Descripcion { get; set; }
        public string? Categoria { get; set; }
        public DateTime? Fecha { get; set; }
        public int Lugar { get; set; }
        public AsistenteEntity? InformacionAsistente { get; set; }
        public FacultadEntity? FacultadesOrganizadoras { get; set; }
        public ComentarioEntity? Comentarios { get; set; }
    }
}
