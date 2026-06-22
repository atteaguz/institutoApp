namespace Academico.DTOs.Estudiante
{
    public class EstudianteResponseDTO
    {
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}