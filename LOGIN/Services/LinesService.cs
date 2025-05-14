using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Lines;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Services
{
    public class LinesService : ILinesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LinesService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<LinesDto>> GetByIdLineAsync(Guid id)
        {
            var entity = await _context.Lines.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<LinesDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Línea no encontrada",
                    Data = null
                };
            }
            return new ResponseDto<LinesDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Lineas obtenidas correctamente",
                Data = _mapper.Map<LinesDto>(entity)
            };
        }

        public async Task<ResponseDto<IEnumerable<LinesDto>>> GetAllLineAsync()
        {
            var entities = await _context.Lines.ToListAsync();
            return new ResponseDto<IEnumerable<LinesDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Lineas obtenidas correctamente",
                Data = _mapper.Map<IEnumerable<LinesDto>>(entities)
            };
        }

        public async Task<ResponseDto<LinesDto>> CreateLine(LinesCreateDto createDto)
        {
            var entity = _mapper.Map<LinesEntity>(createDto);
            _context.Lines.Add(entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<LinesDto>
            {
                Status = true,
                StatusCode = 201,
                Message = "Linea creada correctamente",
                Data = _mapper.Map<LinesDto>(entity)
            };
        }

        public async Task<ResponseDto<LinesDto>> UpdateLine(Guid id, LinesCreateDto updateDto)
        {
            var entity = await _context.Lines.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<LinesDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "No se actualizo la linea",
                    Data = null
                };
            }

            _mapper.Map(updateDto, entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<LinesDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Linea actualizada correctamente",
                Data = _mapper.Map<LinesDto>(entity)
            };
        }
    }
}