using Academico.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academico.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        //entidades a mapear en la base de datos
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //relacion de estudiante y matricula
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Estudiante)
                .WithMany(e => e.Matriculas)
                .HasForeignKey(m => m.EstudianteId)
                .OnDelete(DeleteBehavior.Restrict);

            //relacion de curso y matricula
            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(m => m.CursoId)
                .OnDelete(DeleteBehavior.Restrict);

            //cedula unica
            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.Cedula)
                .IsUnique()
                .HasDatabaseName("UQ_Estudiante_Cedula");

            //correo unico si se ingreso
            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.CorreoElectronico)
                .IsUnique()
                .HasDatabaseName("UQ_Estudiante_Correo")
                .HasFilter("[CorreoElectronico] IS NOT NULL");

            //nombre unico del curso
            modelBuilder.Entity<Curso>()
                .HasIndex(c => c.Nombre)
                .IsUnique()
                .HasDatabaseName("UQ_Curso_Nombre");

            //estudiante y curso unicos para evitar duplicados
            modelBuilder.Entity<Matricula>()
                .HasIndex(m => new { m.EstudianteId, m.CursoId })
                .IsUnique()
                .HasDatabaseName("UQ_Matricula_Estudiante_Curso");

            //valores por defecto
            modelBuilder.Entity<Estudiante>()
                .Property(e => e.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Curso>()
                .Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Matricula>()
                .Property(m => m.FechaMatricula)
                .HasDefaultValueSql("GETDATE()");

            base.OnModelCreating(modelBuilder);
        }
    }
}
