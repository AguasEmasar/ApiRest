using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.Communicates;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Services
{
    public class ComunicateServices : IComunicateServices
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ComunicateServices> _logger;

        public ComunicateServices(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger<ComunicateServices> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto<CommunicateDto>> CreateCommunicate(CreateCommunicateDto model)
        {
            try
            {
                var communicateEntity = _mapper.Map<CommunicateEntity>(model);
                communicateEntity.Date = DateTime.UtcNow;

                _dbContext.Communicates.Add(communicateEntity);
                await _dbContext.SaveChangesAsync();

                var communicateDto = _mapper.Map<CommunicateDto>(communicateEntity);

                _logger.LogInformation("Comunicado creado exitosamente con ID: {CommunicateId}", communicateEntity.Id);
                return new ResponseDto<CommunicateDto>
                {
                    Status = true,
                    StatusCode = 201,
                    Message = "Communiqué created successfully",
                    Data = communicateDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el comunicado: {ErrorMessage}", ex.Message);
                return new ResponseDto<CommunicateDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al crear el comunicado.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<List<CommunicateDto>>> GetAllCommunicates()
        {
            try
            {
                var communicates = await _dbContext.Communicates.ToListAsync();
                var communicatesDto = _mapper.Map<List<CommunicateDto>>(communicates);

                return new ResponseDto<List<CommunicateDto>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "List of communications",
                    Data = communicatesDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<CommunicateDto>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al obtener los comunicados.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<CommunicateDto>> GetCommunicateById(Guid id)
        {
            try
            {
                var communicateEntity = await _dbContext.Communicates.FirstOrDefaultAsync(x => x.Id == id);

                if (communicateEntity == null)
                {
                    return new ResponseDto<CommunicateDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Communication not found"
                    };
                }

                var communicateDto = _mapper.Map<CommunicateDto>(communicateEntity);

                return new ResponseDto<CommunicateDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Communication found",
                    Data = communicateDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<CommunicateDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al buscar el comunicado.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<CommunicateDto>> UpdateCommunicate(CommunicateDto model)
        {
            try
            {
                var communicateEntity = await _dbContext.Communicates.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (communicateEntity == null)
                {
                    return new ResponseDto<CommunicateDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Communication not found"
                    };
                }

                communicateEntity.Tittle = model.Tittle;
                communicateEntity.Content = model.Content;
                communicateEntity.Date = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                var communicateDto = _mapper.Map<CommunicateDto>(communicateEntity);

                _logger.LogInformation("Comunicado actualizado exitosamente con ID: {CommunicateId}", model.Id);
                return new ResponseDto<CommunicateDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Communiqué updated correctly",
                    Data = communicateDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el comunicado con ID: {CommunicateId}", model.Id);
                return new ResponseDto<CommunicateDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al actualizar el comunicado.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<CommunicateDto>> DeleteCommunicate(Guid id)
        {
            try
            {
                var communicateEntity = await _dbContext.Communicates.FirstOrDefaultAsync(x => x.Id == id);

                if (communicateEntity == null)
                {
                    return new ResponseDto<CommunicateDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Communication not found"
                    };
                }

                _dbContext.Communicates.Remove(communicateEntity);
                await _dbContext.SaveChangesAsync();

                var communicateDto = _mapper.Map<CommunicateDto>(communicateEntity);

                _logger.LogInformation("Comunicado eliminado exitosamente con ID: {CommunicateId}", id);
                return new ResponseDto<CommunicateDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Communiqué successfully deleted",
                    Data = communicateDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el comunicado con ID: {CommunicateId}", id);
                return new ResponseDto<CommunicateDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al eliminar el comunicado.",
                    Data = null
                };
            }
        }
    }
}