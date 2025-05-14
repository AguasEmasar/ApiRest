using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class RegistrationWaterService : IRegistrationWaterService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IRegistrationWaterNeighborhoodsColoniesService _registrationWaterNeighborhoodsColoniesService;
    private readonly ILogger<RegistrationWaterService> _logger;

    public RegistrationWaterService(
        ApplicationDbContext context,
        IMapper mapper,
        IRegistrationWaterNeighborhoodsColoniesService registrationWaterNeighborhoodsColoniesService,
        ILogger<RegistrationWaterService> logger)
    {
        _context = context;
        _mapper = mapper;
        _registrationWaterNeighborhoodsColoniesService = registrationWaterNeighborhoodsColoniesService;
        _logger = logger;
    }

    public async Task<ResponseDto<RegistrationWaterDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await _context.RegistrationWater
                .Include(rw => rw.RegistrationWaterNeighborhoodsColonies)
                .ThenInclude(rwnc => rwnc.NeighborhoodsColonies)
                .FirstOrDefaultAsync(rw => rw.Id == id);

            if (entity == null)
            {
                return new ResponseDto<RegistrationWaterDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "El registro no se encontró",
                    Data = null
                };
            }

            var dto = _mapper.Map<RegistrationWaterDto>(entity);
            dto.NeighborhoodColonies = entity.RegistrationWaterNeighborhoodsColonies
                .Select(rwnc => new NeighborhoodsColoniesDto
                {
                    Id = rwnc.NeighborhoodsColonies.Id,
                    Name = rwnc.NeighborhoodsColonies.Name,
                    BlockId = rwnc.NeighborhoodsColonies.BlockId
                }).ToList();

            return new ResponseDto<RegistrationWaterDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Se obtuvo el registro correctamente",
                Data = dto
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<RegistrationWaterDto>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al buscar el registro.",
                Data = null
            };
        }
    }

    public async Task<ResponseDto<IEnumerable<RegistrationWaterDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _context.RegistrationWater
                .Include(rw => rw.RegistrationWaterNeighborhoodsColonies)
                .ThenInclude(rwnc => rwnc.NeighborhoodsColonies)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<RegistrationWaterDto>>(entities);

            return new ResponseDto<IEnumerable<RegistrationWaterDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Se obtuvieron los registros correctamente",
                Data = dtos
            };
        }
        catch (Exception ex)
        {
            return new ResponseDto<IEnumerable<RegistrationWaterDto>>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al obtener los registros.",
                Data = null
            };
        }
    }

    public async Task<ResponseDto<RegistrationWaterDto>> CreateAsync(RegistrationWaterCreateDto createDto)
    {
        try
        {
            var entity = _mapper.Map<RegistrationWaterEntity>(createDto);
            entity.Id = Guid.NewGuid();
            _context.RegistrationWater.Add(entity);
            await _context.SaveChangesAsync();

            if (createDto.NeighborhoodColoniesId != null && createDto.NeighborhoodColoniesId.Any())
            {
                var relations = createDto.NeighborhoodColoniesId.Select(id => new RegistrationWaterNeighborhoodsColoniesEntity
                {
                    Id = Guid.NewGuid(),
                    RegistrationWaterId = entity.Id,
                    NeighborhoodColoniesId = id
                }).ToList();

                await _context.RegistrationWaterNeighborhoodsColonies.AddRangeAsync(relations);
                await _context.SaveChangesAsync();
            }

            var dto = _mapper.Map<RegistrationWaterDto>(entity);

            _logger.LogInformation("Registro de agua creado exitosamente con ID: {RegistrationWaterId}", entity.Id);
            return new ResponseDto<RegistrationWaterDto>
            {
                Status = true,
                StatusCode = 201,
                Message = "Se creó el registro correctamente",
                Data = dto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear el registro de agua: {ErrorMessage}", ex.Message);
            return new ResponseDto<RegistrationWaterDto>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al crear el registro.",
                Data = null
            };
        }
    }

    public async Task<ResponseDto<RegistrationWaterDto>> UpdateAsync(Guid id, RegistrationWaterCreateDto updateDto)
    {
        try
        {
            var entity = await _context.RegistrationWater
                .Include(rw => rw.RegistrationWaterNeighborhoodsColonies)
                .FirstOrDefaultAsync(rw => rw.Id == id);

            if (entity == null)
            {
                return new ResponseDto<RegistrationWaterDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "El registro no se encontró",
                    Data = null
                };
            }

            entity.Date = updateDto.Date;
            entity.Observations = updateDto.Observations;

            _context.RegistrationWaterNeighborhoodsColonies.RemoveRange(entity.RegistrationWaterNeighborhoodsColonies);

            if (updateDto.NeighborhoodColoniesId != null && updateDto.NeighborhoodColoniesId.Any())
            {
                var newRelations = updateDto.NeighborhoodColoniesId.Select(id => new RegistrationWaterNeighborhoodsColoniesEntity
                {
                    Id = Guid.NewGuid(),
                    RegistrationWaterId = entity.Id,
                    NeighborhoodColoniesId = id
                }).ToList();

                entity.RegistrationWaterNeighborhoodsColonies = newRelations;
            }

            _context.RegistrationWater.Update(entity);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<RegistrationWaterDto>(entity);

            _logger.LogInformation("Registro de agua actualizado exitosamente con ID: {RegistrationWaterId}", id);
            return new ResponseDto<RegistrationWaterDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "El registro se actualizó correctamente",
                Data = dto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el registro de agua con ID: {RegistrationWaterId}", id);
            return new ResponseDto<RegistrationWaterDto>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al actualizar el registro.",
                Data = null
            };
        }
    }

    public async Task<ResponseDto<bool>> DeleteAsync(Guid id)
    {
        try
        {
            var entity = await _context.RegistrationWater
                .Include(rw => rw.RegistrationWaterNeighborhoodsColonies)
                .FirstOrDefaultAsync(rw => rw.Id == id);

            if (entity == null)
            {
                return new ResponseDto<bool>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "El registro no se encontró",
                    Data = false
                };
            }

            _context.RegistrationWaterNeighborhoodsColonies.RemoveRange(entity.RegistrationWaterNeighborhoodsColonies);
            _context.RegistrationWater.Remove(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Registro de agua eliminado exitosamente con ID: {RegistrationWaterId}", id);
            return new ResponseDto<bool>
            {
                Status = true,
                StatusCode = 200,
                Message = "El registro se eliminó correctamente",
                Data = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el registro de agua con ID: {RegistrationWaterId}", id);
            return new ResponseDto<bool>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al eliminar el registro.",
                Data = false
            };
        }
    }
}