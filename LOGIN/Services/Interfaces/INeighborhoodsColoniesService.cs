using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;

namespace LOGIN.Services.Interfaces
{
    public interface INeighborhoodsColoniesService
    {
        Task<ResponseDto<NeighborhoodsColoniesDto>> CreateNC(NeighborhoodsColoniesCreateDto createDto);
        Task<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>> GetAllNCAsync();
        Task<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>> GetByBlockIdAsync(Guid blockId);
        Task<ResponseDto<NeighborhoodsColoniesDto>> GetByIdNCAsync(Guid id);
        Task<ResponseDto<NeighborhoodsColoniesDto>> UpdateNC(Guid id, NeighborhoodsColoniesCreateDto updateDto);
    }
}
