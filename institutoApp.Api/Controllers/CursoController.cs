using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.DTOs.Curso;
using inaApp.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Academico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursoController : Controller
    {
        private readonly IGenericService<CursoResponseDTO, CursoCreateDTO, CursoUpdateDTO> _cursoService;

        public CursoController(IGenericService<CursoResponseDTO, CursoCreateDTO, CursoUpdateDTO> cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var response = await _cursoService.ObtenerTodosAsync();
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
                var response = await _cursoService.ObtenerPorIdAsync(id);
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
        public async Task<ActionResult> Create([FromBody] CursoCreateDTO cursoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _cursoService.CrearAsync(cursoDTO);
                return Created("Curso creado", response);
            }
            catch (RequiredFieldMissingException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedCourseNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidCupoException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }

        [HttpPatch("update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CursoUpdateDTO cursoDTO)
        {
            try
            {
                if (id != cursoDTO.Id)
                    return BadRequest("El ID de la URL no coincide con el ID del cuerpo");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _cursoService.ActualizarAsync(cursoDTO);
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
            catch (DuplicatedCourseNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidCupoException ex)
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
                var response = await _cursoService.EliminarAsync(id);
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