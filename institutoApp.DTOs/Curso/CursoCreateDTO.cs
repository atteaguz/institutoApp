using System.ComponentModel.DataAnnotations;

namespace Academico.DTOs.Curso
{
    public class CursoCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string Nombre { get; set; }

        [MaxLength(500, ErrorMessage = "La descripcion no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El maximo de estudiantes es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El maximo de estudiantes debe ser mayor a 0")]
        public int CupoMaximo { get; set; }
    }
}