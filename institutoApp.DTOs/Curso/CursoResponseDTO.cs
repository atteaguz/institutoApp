namespace Academico.DTOs.Curso
{
    public class CursoResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int CupoMaximo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}