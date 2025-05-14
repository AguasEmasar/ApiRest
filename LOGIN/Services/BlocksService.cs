using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Blocks;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.Services
{
    public class BlocksService : IBlocksService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BlocksService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseDto<BlockDto>> GetByIdBloqueAsync(Guid id)
        {
            var entity = await _context.Blocks.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<BlockDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Bloque no encontrado",
                    Data = null
                };
            }
            return new ResponseDto<BlockDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Bloque obtenido correctamente",
                Data = _mapper.Map<BlockDto>(entity)
            };
        }

        public async Task<ResponseDto<IEnumerable<BlockDto>>> GetAllBloqueAsync()
        {
            var entities = await _context.Blocks.ToListAsync();
            return new ResponseDto<IEnumerable<BlockDto>>
            {
                Status = true,
                StatusCode = 200,
                Message = "Se obtubieron los bloqes correctamente",
                Data = _mapper.Map<IEnumerable<BlockDto>>(entities)
            };
        }

        public async Task<ResponseDto<BlockDto>> CreateBloque(BlockCreateDto createDto)
        {
            var entity = _mapper.Map<BlocksEntity>(createDto);
            _context.Blocks.Add(entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<BlockDto>
            {
                Status = true,
                StatusCode = 201,
                Message = "Bloque creado correctamente",
                Data = _mapper.Map<BlockDto>(entity)
            };
        }

        public async Task<ResponseDto<BlockDto>> UpdateAsync(Guid id, BlockCreateDto updateDto)
        {
            var entity = await _context.Blocks.FindAsync(id);
            if (entity == null)
            {
                return new ResponseDto<BlockDto>
                {
                    Status = false,
                    StatusCode = 404,
                    Message = "Bloque no encontrado",
                    Data = null
                };
            }

            _mapper.Map(updateDto, entity);
            await _context.SaveChangesAsync();
            return new ResponseDto<BlockDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Bloque actualizado correctamente",
                Data = _mapper.Map<BlockDto>(entity)
            };
        }
    }
}