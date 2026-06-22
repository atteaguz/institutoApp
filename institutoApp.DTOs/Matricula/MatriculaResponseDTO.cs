namespace Academico.DTOs.Matricula
{
    public class MatriculaResponseDTO
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public int CursoId { get; set; }
        public DateTime FechaMatricula { get; set; }

        //datos para relaciones
        public string EstudianteNombreCompleto { get; set; }
        public string EstudianteCedula { get; set; }
        public string CursoNombre { get; set; }
        public int CursoCupoMaximo { get; set; }
    }
}