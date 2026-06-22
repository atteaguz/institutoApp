using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.DTOs.Estudiante;
using inaApp.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Academico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudianteController : Controller
    {
        private readonly IGenericService<EstudianteResponseDTO, EstudianteCreateDTO, EstudianteUpdateDTO> _estudianteService;

        public EstudianteController(IGenericService<EstudianteResponseDTO, EstudianteCreateDTO, EstudianteUpdateDTO> estudianteService)
        {
            _estudianteService = estudianteService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var response = await _estudianteService.ObtenerTodosAsync();
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                var response = await _estudianteService.ObtenerPorIdAsync(id);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] EstudianteCreateDTO estudianteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _estudianteService.CrearAsync(estudianteDTO);
                return Created("Estudiante creado",response);
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedCedulaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        [HttpPatch("update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EstudianteUpdateDTO estudianteDTO)
        {
            try
            {
                if (id != estudianteDTO.Id)
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _estudianteService.ActualizarAsync(estudianteDTO);
                return Ok(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedCedulaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var response = await _estudianteService.EliminarAsync(id);
                return response.Success ? Ok(response) : BadRequest(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }
    }
}