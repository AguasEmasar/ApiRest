using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;

namespace LOGIN.Services.Interfaces
{
    public interface IRegistrationWaterNeighborhoodsColoniesService
    {
        Task AddRangeAsync(IEnumerable<RegistrationWaterNeighborhoodsColoniesDto> dtos);
        Task<RegistrationWaterNeighborhoodsColoniesDto> GetByIdAsync(Guid id);
        Task<ResponseDto<IEnumerable<RegistrationWaterNeighborhoodsColoniesDto>>> GetByNeighborhoodsColoniesAsync(IEnumerable<Guid> neighborhoodColoniesIds);
    }
}
