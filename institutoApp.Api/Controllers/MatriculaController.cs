using Academico.Common.Exceptions;
using Academico.Common.Interfaces;
using Academico.DTOs.Matricula;
using inaApp.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Academico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculaController : Controller
    {
        //matricula no tiene UpdateDTO
        //matriculas no se pueden eliminar segun requerimentos

        private readonly IGenericService<MatriculaResponseDTO, MatriculaCreateDTO, object> _matriculaService;

        public MatriculaController(IGenericService<MatriculaResponseDTO, MatriculaCreateDTO, object> matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var response = await _matriculaService.ObtenerTodosAsync();
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
                var response = await _matriculaService.ObtenerPorIdAsync(id);
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
        public async Task<ActionResult> Create([FromBody] MatriculaCreateDTO matriculaDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _matriculaService.CrearAsync(matriculaDTO);
                return Created("Matricula creada", response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InactiveEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DuplicatedMatriculaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CourseFullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor. Contacte con el administrador");
            }
        }
    }
}