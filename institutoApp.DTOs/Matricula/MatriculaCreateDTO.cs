using System.ComponentModel.DataAnnotations;

namespace Academico.DTOs.Matricula
{
    public class MatriculaCreateDTO
    {
        [Required(ErrorMessage = "El ID del estudiante es obligatorio")]
        public int EstudianteId { get; set; }

        [Required(ErrorMessage = "El ID del curso es obligatorio")]
        public int CursoId { get; set; }
    }
}