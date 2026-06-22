using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.Common.Response;
using Academico.DTOs.Curso;
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
    public class CursoService : IGenericService<CursoResponseDTO, CursoCreateDTO, CursoUpdateDTO>
    {
        private readonly CursoRepository _cursoRepo;
        private readonly IMapper _mapper;

        public CursoService(CursoRepository cursoRepo, IMapper mapper)
        {
            _cursoRepo = cursoRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<CursoResponseDTO>>> ObtenerTodosAsync()
        {
            var cursos = await _cursoRepo.ObtenerTodosAsync();

            if (!cursos.Any())
                throw new NotFoundException("No se encontraron cursos");

            return new Response<List<CursoResponseDTO>>
            {
                Data = _mapper.Map<List<CursoResponseDTO>>(cursos),
                Message = "Cursos obtenidos correctamente",
                Success = true
            };
        }

        public async Task<Response<CursoResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var curso = await _cursoRepo.ObtenerPorIdAsync(id);

            if (curso == null)
                throw new NotFoundException($"Curso con ID {id} no encontrado");

            return new Response<CursoResponseDTO>
            {
                Data = _mapper.Map<CursoResponseDTO>(curso),
                Message = "Curso obtenido correctamente",
                Success = true
            };
        }

        public async Task<Response<CursoResponseDTO>> CrearAsync(CursoCreateDTO entity)
        {
            //nombre obligatorio
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldMissingException("El nombre del curso es obligatorio");

            //nombre unico
            if (await _cursoRepo.ExisteNombreAsync(entity.Nombre))
                throw new DuplicatedCourseNameException($"El nombre '{entity.Nombre}' ya esta registrado");

            //cupo maximo mayor que cero
            if (entity.CupoMaximo <= 0)
                throw new InvalidCupoException("El cupo maximo debe ser mayor a 0");

            //no cursos duplicados ya esta validado por el nombre unico

            var curso = _mapper.Map<Curso>(entity);
            curso = await _cursoRepo.CrearAsync(curso);

            return new Response<CursoResponseDTO>
            {
                Data = _mapper.Map<CursoResponseDTO>(curso),
                Message = "Curso creado exitosamente",
                Success = true
            };
        }

        public async Task<Response<CursoResponseDTO>> ActualizarAsync(CursoUpdateDTO entity)
        {
            //validar que el curso existe
            var cursoExistente = await _cursoRepo.ObtenerPorIdAsync(entity.Id);
            if (cursoExistente == null)
                throw new NotFoundException($"Curso con ID {entity.Id} no encontrado");

            //nombre obligatorio
            if (string.IsNullOrWhiteSpace(entity.Nombre))
                throw new RequiredFieldMissingException("El nombre del curso es obligatorio");

            //nombre uncico (se excluye al curso actual para actualizar)
            if (await _cursoRepo.ExisteNombreAsync(entity.Nombre, entity.Id))
                throw new DuplicatedCourseNameException($"El nombre '{entity.Nombre}' ya esta registrado por otro curso");

            //cupo maximo mayor que cero
            if (entity.CupoMaximo <= 0)
                throw new InvalidCupoException("El cupo maximo debe ser mayor a 0");

            var curso = _mapper.Map<Curso>(entity);
            curso = await _cursoRepo.ActualizarAsync(curso);

            return new Response<CursoResponseDTO>
            {
                Data = _mapper.Map<CursoResponseDTO>(curso),
                Message = "Curso actualizado exitosamente",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var curso = await _cursoRepo.ObtenerPorIdAsync(id);
            if (curso == null)
                throw new NotFoundException($"Curso con ID {id} no encontrado");

            var result = await _cursoRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = result,
                Message = result ? "Curso eliminado exitosamente" : "Error al eliminar el curso",
                Success = result
            };
        }
    }
}