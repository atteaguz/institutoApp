using AutoMapper;
using Academico.DTOs.Estudiante;
using Academico.DTOs.Curso;
using Academico.DTOs.Matricula;
using Academico.Entities;

namespace Academico.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //mapping de estudiante
            CreateMap<EstudianteCreateDTO, Estudiante>();
            CreateMap<EstudianteUpdateDTO, Estudiante>();
            CreateMap<Estudiante, EstudianteResponseDTO>();

            //mapping de curso
            CreateMap<CursoCreateDTO, Curso>();
            CreateMap<CursoUpdateDTO, Curso>();
            CreateMap<Curso, CursoResponseDTO>();

            //mapping de matricula
            CreateMap<MatriculaCreateDTO, Matricula>();
            CreateMap<Matricula, MatriculaResponseDTO>()
                .ForMember(dest => dest.EstudianteNombreCompleto,
                    opt => opt.MapFrom(src =>
                        $"{src.Estudiante.Nombre} {src.Estudiante.PrimerApellido} {src.Estudiante.SegundoApellido ?? ""}".Trim()))
                .ForMember(dest => dest.EstudianteCedula,
                    opt => opt.MapFrom(src => src.Estudiante.Cedula))
                .ForMember(dest => dest.CursoNombre,
                    opt => opt.MapFrom(src => src.Curso.Nombre))
                .ForMember(dest => dest.CursoCupoMaximo,
                    opt => opt.MapFrom(src => src.Curso.CupoMaximo));
        }
    }
}