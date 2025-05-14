using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LOGIN.Services
{
    public class NeighborhoodsColoniesService : INeighborhoodsColoniesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<NeighborhoodsColoniesService> _logger;

        public NeighborhoodsColoniesService(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<NeighborhoodsColoniesService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto<NeighborhoodsColoniesDto>> GetByIdNCAsync(Guid id)
        {
            try
            {
                var entity = await _context.NeighborhoodsColonies.FindAsync(id);
                if (entity == null)
                {
                    return new ResponseDto<NeighborhoodsColoniesDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Barrio/colonia no encontrada",
                        Data = null
                    };
                }

                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Barrio/colonia obtenida correctamente",
                    Data = _mapper.Map<NeighborhoodsColoniesDto>(entity)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al buscar el barrio/colonia.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>> GetAllNCAsync()
        {
            try
            {
                var entities = await _context.NeighborhoodsColonies.ToListAsync();

                return new ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Barrios/colonias obtenidos correctamente",
                    Data = _mapper.Map<IEnumerable<NeighborhoodsColoniesDto>>(entities)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al obtener los barrios/colonias.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<NeighborhoodsColoniesDto>> CreateNC(NeighborhoodsColoniesCreateDto createDto)
        {
            try
            {
                var entity = _mapper.Map<NeighborhoodsColoniesEntity>(createDto);
                _context.NeighborhoodsColonies.Add(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Barrio/colonia creado exitosamente con ID: {NeighborhoodColonyId}", entity.Id);
                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = true,
                    StatusCode = 201,
                    Message = "Barrio/colonia creado correctamente",
                    Data = _mapper.Map<NeighborhoodsColoniesDto>(entity)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el barrio/colonia: {ErrorMessage}", ex.Message);
                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al crear el barrio/colonia.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<NeighborhoodsColoniesDto>> UpdateNC(Guid id, NeighborhoodsColoniesCreateDto updateDto)
        {
            try
            {
                var entity = await _context.NeighborhoodsColonies.FindAsync(id);
                if (entity == null)
                {
                    return new ResponseDto<NeighborhoodsColoniesDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Barrio/colonia no encontrada",
                        Data = null
                    };
                }

                _mapper.Map(updateDto, entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Barrio/colonia actualizado exitosamente con ID: {NeighborhoodColonyId}", id);
                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Barrio/colonia actualizada correctamente",
                    Data = _mapper.Map<NeighborhoodsColoniesDto>(entity)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el barrio/colonia con ID: {NeighborhoodColonyId}", id);
                return new ResponseDto<NeighborhoodsColoniesDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al actualizar el barrio/colonia.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>> GetByBlockIdAsync(Guid blockId)
        {
            try
            {
                var entities = await _context.NeighborhoodsColonies
                    .Where(nc => nc.BlockId == blockId)
                    .ToListAsync();

                return new ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Barrios/colonias obtenidos correctamente",
                    Data = _mapper.Map<IEnumerable<NeighborhoodsColoniesDto>>(entities)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al obtener los barrios/colonias.",
                    Data = null
                };
            }
        }
    }
}