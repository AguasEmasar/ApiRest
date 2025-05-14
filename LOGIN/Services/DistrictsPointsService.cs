using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Districts;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Services
{
    public class DistrictsPointsService : IDistrictsPointsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DistrictsPointsService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<DistrictsPointsDto>> GetByIdDistrictsPointsAsync(Guid id)
        {
            var entity = await _context.Districts.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<DistrictsPointsDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Punto de distrito no encontrado",
                    Data = null
                };
            }
            return new ResponseDto<DistrictsPointsDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Punto de distrito obtenido correctamente",
                Data = _mapper.Map<DistrictsPointsDto>(entity)
            };
        }

        public async Task<ResponseDto<IEnumerable<DistrictsPointsDto>>> GetAllDistrictsPointsAsync()
        {
            var entities = await _context.Districts.ToListAsync();
            return new ResponseDto<IEnumerable<DistrictsPointsDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Puntos de distritos encontrado correctamente",
                Data = _mapper.Map<IEnumerable<DistrictsPointsDto>>(entities)
            };
        }

        public async Task<ResponseDto<DistrictsPointsDto>> CreateDistrictsPoints(DistrictsPointsCreateDto createDto)
        {
            var entity = _mapper.Map<DistrictsPointsEntity>(createDto);
            _context.Districts.Add(entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<DistrictsPointsDto>
            {
                Status = true,
                StatusCode = 201,
                Message = "Punto de distrito creado correctamente",
                Data = _mapper.Map<DistrictsPointsDto>(entity)
            };
        }

        public async Task<ResponseDto<DistrictsPointsDto>> UpdateDistrictsPoints(Guid id, DistrictsPointsCreateDto updateDto)
        {
            var entity = await _context.Districts.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<DistrictsPointsDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Punto de distrito no encontrado",
                    Data = null
                };
            }

            _mapper.Map(updateDto, entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<DistrictsPointsDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Punto de distrito actualizado correctamente",
                Data = _mapper.Map<DistrictsPointsDto>(entity)
            };
        }
        public async Task<ResponseDto<IEnumerable<DistrictsPointsDto>>> GetByNeighborhoodsColoniesIdAsync(Guid neighborhoodsColoniesId)
        {
            var entities = await _context.Districts
                .Where(dp => dp.NeighborhoodsColoniesId == neighborhoodsColoniesId)
                .ToListAsync();

            return new ResponseDto<IEnumerable<DistrictsPointsDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Puntos de distrito obtenidos correctamente",
                Data = _mapper.Map<IEnumerable<DistrictsPointsDto>>(entities)
            };
        }
    }
}