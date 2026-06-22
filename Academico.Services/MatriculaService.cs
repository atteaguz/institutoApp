using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.Common.Response;
using Academico.DTOs.Matricula;
using Academico.Entities;
using Academico.Repository;
using AutoMapper;
using inaApp.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academico.Services
{
    public class MatriculaService : IGenericService<MatriculaResponseDTO, MatriculaCreateDTO, object>
    {
        private readonly MatriculaRepository _matriculaRepo;
        private readonly EstudianteRepository _estudianteRepo;
        private readonly CursoRepository _cursoRepo;
        private readonly IMapper _mapper;

        public MatriculaService(
            MatriculaRepository matriculaRepo,
            EstudianteRepository estudianteRepo,
            CursoRepository cursoRepo,
            IMapper mapper)
        {
            _matriculaRepo = matriculaRepo;
            _estudianteRepo = estudianteRepo;
            _cursoRepo = cursoRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<MatriculaResponseDTO>>> ObtenerTodosAsync()
        {
            var matriculas = await _matriculaRepo.ObtenerTodosAsync();

            if (!matriculas.Any())
                throw new NotFoundException("No se encontraron matriculas");

            return new Response<List<MatriculaResponseDTO>>
            {
                Data = _mapper.Map<List<MatriculaResponseDTO>>(matriculas),
                Message = "Matriculas obtenidas correctamente",
                Success = true
            };
        }

        public async Task<Response<MatriculaResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var matricula = await _matriculaRepo.ObtenerPorIdAsync(id);

            if (matricula == null)
                throw new NotFoundException($"Matricula con ID {id} no encontrada");

            return new Response<MatriculaResponseDTO>
            {
                Data = _mapper.Map<MatriculaResponseDTO>(matricula),
                Message = "Matricula obtenida correctamente",
                Success = true
            };
        }

        public async Task<Response<MatriculaResponseDTO>> CrearAsync(MatriculaCreateDTO entity)
        {
            //el estudiante debe existir
            var estudiante = await _estudianteRepo.ObtenerPorIdAsync(entity.EstudianteId);
            if (estudiante == null)
                throw new NotFoundException($"Estudiante con ID {entity.EstudianteId} no encontrado");

            //el curso debe existir
            var curso = await _cursoRepo.ObtenerPorIdAsync(entity.CursoId);
            if (curso == null)
                throw new NotFoundException($"Curso con ID {entity.CursoId} no encontrado");

            //el estudiante debe estar activo
            if (!estudiante.Estado)
                throw new InactiveEntityException($"El estudiante '{estudiante.Nombre} {estudiante.PrimerApellido}' esta inactivo");

            //cursos deben estar activos
            if (!curso.Estado)
                throw new InactiveEntityException($"El curso '{curso.Nombre}' esta inactivo");

            //no permitir matriculas duplicadas
            if (await _matriculaRepo.ExisteMatriculaAsync(entity.EstudianteId, entity.CursoId))
                throw new DuplicatedMatriculaException($"El estudiante ya esta matriculado en el curso '{curso.Nombre}'");

            //no permitir exceder el cupo máximo
            var cupoOcupado = await _matriculaRepo.ObtenerCupoOcupadoAsync(entity.CursoId);
            if (cupoOcupado >= curso.CupoMaximo)
                throw new CourseFullException($"El curso '{curso.Nombre}' ha alcanzado su cupo maximo de {curso.CupoMaximo} estudiantes");

            var matricula = _mapper.Map<Matricula>(entity);
            matricula = await _matriculaRepo.CrearAsync(matricula);

            //response con relaciones
            matricula = await _matriculaRepo.ObtenerPorIdAsync(matricula.Id);

            return new Response<MatriculaResponseDTO>
            {
                Data = _mapper.Map<MatriculaResponseDTO>(matricula),
                Message = "Matricula creada exitosamente",
                Success = true
            };
        }

        //matricula no tiene UpdateDTO
        public async Task<Response<MatriculaResponseDTO>> ActualizarAsync(object entity)
        {
            throw new NotImplementedException("Las matriculas no se pueden actualizar");
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var matricula = await _matriculaRepo.ObtenerPorIdAsync(id);
            if (matricula == null)
                throw new NotFoundException($"Matricula con ID {id} no encontrada");

            var result = await _matriculaRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = result,
                Message = result ? "Matricula eliminada exitosamente" : "Error al eliminar la matricula",
                Success = result
            };
        }
    }
}