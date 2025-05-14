using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Districts;

namespace LOGIN.Services.Interfaces
{
    public interface IDistrictsPointsService
    {
        Task<ResponseDto<DistrictsPointsDto>> CreateDistrictsPoints(DistrictsPointsCreateDto createDto);
        Task<ResponseDto<IEnumerable<DistrictsPointsDto>>> GetAllDistrictsPointsAsync();
        Task<ResponseDto<DistrictsPointsDto>> GetByIdDistrictsPointsAsync(Guid id);
        Task<ResponseDto<IEnumerable<DistrictsPointsDto>>> GetByNeighborhoodsColoniesIdAsync(Guid neighborhoodsColoniesId);
        Task<ResponseDto<DistrictsPointsDto>> UpdateDistrictsPoints(Guid id, DistrictsPointsCreateDto updateDto);
    }
}
