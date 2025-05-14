using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Blocks;

namespace LOGIN.Services.Interfaces
{
    public interface IBlocksService
    {
        Task<ResponseDto<BlockDto>> CreateBloque(BlockCreateDto createDto);
        Task<ResponseDto<IEnumerable<BlockDto>>> GetAllBloqueAsync();
        Task<ResponseDto<BlockDto>> GetByIdBloqueAsync(Guid id);
        Task<ResponseDto<BlockDto>> UpdateAsync(Guid id, BlockCreateDto updateDto);
    }
}
