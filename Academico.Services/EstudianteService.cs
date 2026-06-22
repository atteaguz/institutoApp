using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.Common.Response;
using Academico.DTOs.Estudiante;
using Academico.Entities;
using Academico.Repository;
using AutoMapper;
using inaApp.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Academico.Services
{
    public class EstudianteService : IGenericService<EstudianteResponseDTO, EstudianteCreateDTO, EstudianteUpdateDTO>
    {
        private readonly EstudianteRepository _estudianteRepo;
        private readonly IMapper _mapper;

        public EstudianteService(EstudianteRepository estudianteRepo, IMapper mapper)
        {
            _estudianteRepo = estudianteRepo;
            _mapper = mapper;
        }

        public async Task<Response<List<EstudianteResponseDTO>>> ObtenerTodosAsync()
        {
            var estudiantes = await _estudianteRepo.ObtenerTodosAsync();

            if (!estudiantes.Any())
                throw new NotFoundException("No se encontraron estudiantes");

            return new Response<List<EstudianteResponseDTO>>
            {
                Data = _mapper.Map<List<EstudianteResponseDTO>>(estudiantes),
                Message = "Estudiantes obtenidos correctamente",
                Success = true
            };
        }

        public async Task<Response<EstudianteResponseDTO>> ObtenerPorIdAsync(int id)
        {
            var estudiante = await _estudianteRepo.ObtenerPorIdAsync(id);

            if (estudiante == null)
                throw new NotFoundException($"Estudiante con ID {id} no encontrado");

            return new Response<EstudianteResponseDTO>
            {
                Data = _mapper.Map<EstudianteResponseDTO>(estudiante),
                Message = "Estudiante obtenido correctamente",
                Success = true
            };
        }

        public async Task<Response<EstudianteResponseDTO>> CrearAsync(EstudianteCreateDTO entity)
        {
            //cedula obligatoria
            if (string.IsNullOrWhiteSpace(entity.Cedula))
                throw new RequiredFieldMissingException("La cedula es obligatoria");

            //cedula unica
            if (await _estudianteRepo.ExisteCedulaAsync(entity.Cedula))
                throw new DuplicatedCedulaException($"La cedula '{entity.Cedula}' ya esta registrada");

            //correo electronico con formaato valido ya esta validado

            //correo debe ser unico
            if (!string.IsNullOrEmpty(entity.CorreoElectronico))
            {
                if (await _estudianteRepo.ExisteCorreoAsync(entity.CorreoElectronico))
                    throw new DuplicatedEmailException($"El correo '{entity.CorreoElectronico}' ya esta registrado");
            }

            //no permitir estudiantes duplicados ya validado por cedula unica

            var estudiante = _mapper.Map<Estudiante>(entity);
            estudiante = await _estudianteRepo.CrearAsync(estudiante);

            return new Response<EstudianteResponseDTO>
            {
                Data = _mapper.Map<EstudianteResponseDTO>(estudiante),
                Message = "Estudiante creado exitosamente",
                Success = true
            };
        }

        public async Task<Response<EstudianteResponseDTO>> ActualizarAsync(EstudianteUpdateDTO entity)
        {
            //validar que el estudiante existe
            var estudianteExistente = await _estudianteRepo.ObtenerPorIdAsync(entity.Id);
            if (estudianteExistente == null)
                throw new NotFoundException($"Estudiante con ID {entity.Id} no encontrado");

            //cedula obligatoria
            if (string.IsNullOrWhiteSpace(entity.Cedula))
                throw new RequiredFieldMissingException("La cedula es obligatoria");

            //cedula unica (se excluye al estudiante actual para la actualizacion)
            if (await _estudianteRepo.ExisteCedulaAsync(entity.Cedula, entity.Id))
                throw new DuplicatedCedulaException($"La cedula '{entity.Cedula}' ya esta registrada por otro estudiante");

            //correo electronico con formaato valido ya esta validado

            //correo unico (se excluye al estudiante actual para la actualizacion)
            if (!string.IsNullOrEmpty(entity.CorreoElectronico))
            {
                if (await _estudianteRepo.ExisteCorreoAsync(entity.CorreoElectronico, entity.Id))
                    throw new DuplicatedEmailException($"El correo '{entity.CorreoElectronico}' ya esta registrado por otro estudiante");
            }

            var estudiante = _mapper.Map<Estudiante>(entity);
            estudiante = await _estudianteRepo.ActualizarAsync(estudiante);

            return new Response<EstudianteResponseDTO>
            {
                Data = _mapper.Map<EstudianteResponseDTO>(estudiante),
                Message = "Estudiante actualizado exitosamente",
                Success = true
            };
        }

        public async Task<Response<bool>> EliminarAsync(int id)
        {
            var estudiante = await _estudianteRepo.ObtenerPorIdAsync(id);
            if (estudiante == null)
                throw new NotFoundException($"Estudiante con ID {id} no encontrado");

            var result = await _estudianteRepo.EliminarAsync(id);

            return new Response<bool>
            {
                Data = result,
                Message = result ? "Estudiante eliminado exitosamente" : "Error al eliminar el estudiante",
                Success = result
            };
        }
    }
}