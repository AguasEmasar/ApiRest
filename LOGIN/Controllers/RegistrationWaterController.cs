using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using LOGIN.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LOGIN.Services.Interfaces;
using LOGIN.Dtos.NotificationDtos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;

[ApiController]
[Route("api/registration")]
public class RegistrationWaterController : ControllerBase
{
    private readonly IRegistrationWaterService _registrationWaterService;
    private readonly INotificationService _notificationService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegistrationWaterController(
        IRegistrationWaterService registrationWaterService, 
        INotificationService notificationService,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _registrationWaterService = registrationWaterService;
        _notificationService = notificationService;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseDto<RegistrationWaterDto>>> GetByIdRegistrationAsync(Guid id)
    {
        var result = await _registrationWaterService.GetByIdAsync(id);
        if (result.Status)
        {
            return Ok(result);
        }
        return StatusCode(result.StatusCode, result);
    }


    [HttpGet]
    public async Task<ActionResult<ResponseDto<IEnumerable<RegistrationWaterDto>>>> GetAllRegistrationAsync()
    {
        var result = await _registrationWaterService.GetAllAsync();
        return StatusCode(result.StatusCode, result);
    }

    //authorizar segun el rol
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ResponseDto<RegistrationWaterDto>>> CreateRegistration([FromBody] RegistrationWaterCreateDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseDto<string>
            {
                Status = false,
                StatusCode = 400,
                Message = "El modelo es inválido",
                Data = ModelState.ToString()
            });
        }

        var result = await _registrationWaterService.CreateAsync(createDto);

        if (!result.Status)
        {
            return StatusCode(result.StatusCode, result);
        }

        // Mapear y enviar notificación
        try
        {
            var registrationWaterDto = _mapper.Map<RegistrationWaterDto>(result.Data);

            // Asegurarnos que NeighborhoodColonies esté cargado
            if (registrationWaterDto.NeighborhoodColonies == null || !registrationWaterDto.NeighborhoodColonies.Any())
            {
                var fullData = await _context.RegistrationWater
                    .Include(r => r.RegistrationWaterNeighborhoodsColonies)
                    .FirstOrDefaultAsync(r => r.Id == registrationWaterDto.Id);

                if (fullData != null)
                {
                    registrationWaterDto.NeighborhoodColonies = _mapper.Map<List<NeighborhoodsColoniesDto>>(fullData.RegistrationWaterNeighborhoodsColonies);
                }
            }

            // Enviar notificación en segundo plano
            _ = _notificationService.SendWaterRegistrationNotificationAsync(registrationWaterDto);

            return Ok(new
            {
                result.Status,
                result.Message,
                Data = new
                {
                    registrationWaterDto.Id,
                    NeighborhoodColonies = registrationWaterDto.NeighborhoodColonies?.Select(n => n.Name),
                    registrationWaterDto.Observations,
                    registrationWaterDto.Date,
                    NotificationStatus = "Enviada"
                }
            });
        }
        catch (Exception ex)
        {
            // Loggear el error pero seguir retornando la respuesta exitosa
            Console.WriteLine($"Error al enviar notificación: {ex.Message}");

            return Ok(new
            {
                result.Status,
                result.Message,
                Data = new
                {
                    result.Data.Id,
                    NeighborhoodColonies = result.Data.NeighborhoodColonies?.Select(n => n.Name),
                    result.Data.Observations,
                    result.Data.Date,
                    NotificationStatus = "Error al enviar"
                }
            });
        }
    }
    //test de notificacion
    [HttpGet("test-notification")]
    public async Task<IActionResult> TestNotification()
    {
        try
        {
            var testData = new RegistrationWaterDto
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Observations = "Esta es una prueba",
                NeighborhoodColonies = new List<NeighborhoodsColoniesDto>
            {
                new NeighborhoodsColoniesDto { Name = "Colonia de prueba 1" },
                new NeighborhoodsColoniesDto { Name = "Colonia de prueba 2" }
            }
            };

            await _notificationService.SendWaterRegistrationNotificationAsync(testData);
            return Ok("Notificación de prueba enviada");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [Authorize(Roles="Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RegistrationWaterCreateDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _registrationWaterService.UpdateAsync(id, updateDto);
        if (!response.Status)
        {
            return NotFound(response.Message);
        }

        return Ok(response.Data);
    }

    [Authorize(Roles="Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _registrationWaterService.DeleteAsync(id);
        if (!response.Status)
        {
            return NotFound(response.Message);
        }

        return Ok(response.Message);
    }
}
