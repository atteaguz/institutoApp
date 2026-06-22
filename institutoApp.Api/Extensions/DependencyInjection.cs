using Academico.Common.Interfaces;
using Academico.Data;
using Academico.DTOs.Estudiante;
using Academico.DTOs.Curso;
using Academico.DTOs.Matricula;
using Academico.Entities;
using Academico.Repository;
using Academico.Services;
using Academico.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Academico.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //base de datos - dbcontext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            //inyeccion de automapper
            services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

            //inyeccion de repositories
            services.AddScoped<EstudianteRepository>();
            services.AddScoped<CursoRepository>();
            services.AddScoped<MatriculaRepository>();

            //inyeccion de services
            services.AddScoped<IGenericService<EstudianteResponseDTO, EstudianteCreateDTO, EstudianteUpdateDTO>, EstudianteService>();
            services.AddScoped<IGenericService<CursoResponseDTO, CursoCreateDTO, CursoUpdateDTO>, CursoService>();
            services.AddScoped<IGenericService<MatriculaResponseDTO, MatriculaCreateDTO, object>, MatriculaService>();

            return services;
        }
    }
}