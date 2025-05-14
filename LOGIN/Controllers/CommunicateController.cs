using AutoMapper;
using FirebaseAdmin.Messaging;
using LOGIN.Dtos.Communicates;
using LOGIN.Dtos.NotificationDtos;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOGIN.Controllers
{
    [Route("api/communicate")]
    [ApiController]
    public class CommunicateController : ControllerBase
    {
        private readonly IComunicateServices _comunicateServices;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public CommunicateController(IComunicateServices comunicateServices, 
            INotificationService notificationService, IMapper mapper)
        {
            _comunicateServices = comunicateServices;
            _notificationService = notificationService;
            _mapper = mapper;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCommunicate([FromBody] CreateCommunicateDto model)
        {
            // Validar modelo
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = false,
                    StatusCode = 400,
                    Message = "Datos inválidos",
                    Data = ModelState.ToString()
                });
            }

            // Guardar en base de datos
            var response = await _comunicateServices.CreateCommunicate(model);

            if (!response.Status)
            {
                return BadRequest(response);
            }

            // Enviar notificación en segundo plano sin esperar respuesta
            var communicateDto = _mapper.Map<CommunicateDto>(response.Data);
            _ = _notificationService.SendCommunicateNotificationAsync(communicateDto);

            return Ok(new
            {
                response.Status,
                response.Message,
                Data = new
                {
                    communicateDto.Id,
                    communicateDto.Tittle,
                    communicateDto.Type_Statement,
                    communicateDto.Date,
                    NotificationStatus = "Enviada"
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommunicates()
        {
            var response = await _comunicateServices.GetAllCommunicates();

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        //obtener comunicado por id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunicateById(Guid id)
        {
            var response = await _comunicateServices.GetCommunicateById(id);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }


        //editar comunicado
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCommunicate(Guid id, [FromBody] CommunicateDto model)
        {
            model.Id = id;
            var response = await _comunicateServices.UpdateCommunicate(model);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        //eliminar comunicado
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunicate(Guid id)
        {
            var response = await _comunicateServices.DeleteCommunicate(id);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

    }
}