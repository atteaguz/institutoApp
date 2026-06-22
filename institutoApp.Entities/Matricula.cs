using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Academico.Entities
{
    [Table("tbMatricula")]
    public class Matricula
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EstudianteId { get; set; }

        [Required]
        public int CursoId { get; set; }

        [Required]
        public DateTime FechaMatricula { get; set; } = DateTime.Now;

        [Required]
        public bool Estado { get; set; } = true;

        //relaciones de matriculas con estudiantee y cursos
        [ForeignKey("EstudianteId")]
        public Estudiante Estudiante { get; set; }

        [ForeignKey("CursoId")]
        public Curso Curso { get; set; }
    }
}