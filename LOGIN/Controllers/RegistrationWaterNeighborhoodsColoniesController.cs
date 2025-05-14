using Microsoft.AspNetCore.Mvc;
using LOGIN.Dtos;
using LOGIN.Services.Interfaces;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using Microsoft.AspNetCore.Authorization;

namespace LOGIN.Controllers
{
    [ApiController]
    [Route("api/registrationWaterNeighborhoodsColonies")]
    public class RegistrationWaterNeighborhoodsColoniesController : ControllerBase
    {
        private readonly IRegistrationWaterNeighborhoodsColoniesService _service;

        public RegistrationWaterNeighborhoodsColoniesController(IRegistrationWaterNeighborhoodsColoniesService service)
        {
            _service = service;
        }

        [HttpGet("by-neighborhoods-colonies")]
        public async Task<IActionResult> GetByNeighborhoodsColonies([FromQuery] IEnumerable<Guid> neighborhoodsColoniesIds)
        {
            if (neighborhoodsColoniesIds == null || !neighborhoodsColoniesIds.Any())
            {
                return BadRequest(new ResponseDto<string>
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Se deben proporcionar identificadores de barrios/colonias",
                    Data = null
                });
            }

            try
            {
                var response = await _service.GetByNeighborhoodsColoniesAsync(neighborhoodsColoniesIds);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPost("add-relations")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] IEnumerable<RegistrationWaterNeighborhoodsColoniesDto> dtos)
        {
            if (dtos == null || !ModelState.IsValid)
            {
                return BadRequest(new { Status = false, Message = "Datos inválidos", StatusCode = 400 });
            }

            try
            {
                await _service.AddRangeAsync(dtos);
                return Ok(new { Status = true, Message = "Relaciones guardadas exitosamente", StatusCode = 201 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = false, Message = $"Error interno: {ex.Message}", StatusCode = 500 });
            }
        }

        // Método para obtener una relación por ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound(new { Status = false, Message = "No se encontró la relación", StatusCode = 404 });
            }

            return Ok(new { Status = true, Data = entity });
        }
    }
}
