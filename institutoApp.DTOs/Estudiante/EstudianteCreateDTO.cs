using System.ComponentModel.DataAnnotations;

namespace Academico.DTOs.Estudiante
{
    public class EstudianteCreateDTO
    {
        [Required(ErrorMessage = "La cedula es obligatoria")]
        [MaxLength(20, ErrorMessage = "La cédula no puede exceder los 20 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        [MaxLength(100, ErrorMessage = "El primer apellido no puede exceder los 100 caracteres")]
        public string PrimerApellido { get; set; }

        [MaxLength(100, ErrorMessage = "El segundo apellido no puede exceder los 100 caracteres")]
        public string? SegundoApellido { get; set; }

        [MaxLength(150, ErrorMessage = "El correo no puede exceder los 150 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del correo electronico no es vvlido")]
        public string? CorreoElectronico { get; set; }

        [MaxLength(20, ErrorMessage = "El telefono no puede exceder los 20 caracteres")]
        [Phone(ErrorMessage = "El formato del telefono no es valido")]
        public string? Telefono { get; set; }
    }
}