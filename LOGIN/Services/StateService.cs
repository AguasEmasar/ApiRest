using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using LOGIN.Dtos.States;

namespace LOGIN.Services
{
    public class StateService : IStateService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<StateService> _logger;

        public StateService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger<StateService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;

        }

        // Traer todos los estados
        public async Task<ResponseDto<List<StateDto>>> GetAllStatesAsync()
        {
            try
            {
                var statesEntity = await _dbContext.States.ToListAsync();

                if (statesEntity == null || statesEntity.Count == 0)
                {
                    return new ResponseDto<List<StateDto>>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "No se encontraron estados."
                    };
                }

                var statesDto = _mapper.Map<List<StateDto>>(statesEntity);

                _logger.LogInformation("Se encontraron {Count} estados.", statesEntity.Count);
                return new ResponseDto<List<StateDto>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Estados encontrados",
                    Data = statesDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estados.");
                return new ResponseDto<List<StateDto>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al obtener los estados."
                };
            }
        }

        // Traer estado por ID
        public async Task<ResponseDto<StateDto>> GetStateByIdAsync(Guid id)
        {
            try
            {
                var stateEntity = await _dbContext.States.FirstOrDefaultAsync(x => x.Id == id);

                if (stateEntity == null)
                {
                    return new ResponseDto<StateDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Estado no encontrado"
                    };
                }

                var stateDto = _mapper.Map<StateDto>(stateEntity);

                _logger.LogInformation("Estado encontrado con ID: {StateId}", id);
                return new ResponseDto<StateDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Estado encontrado",
                    Data = stateDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar el estado con ID: {StateId}", id);
                return new ResponseDto<StateDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al buscar el estado."
                };
            }
        }
    }
}