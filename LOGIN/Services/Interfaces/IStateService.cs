using LOGIN.Dtos;
using LOGIN.Dtos.States;

namespace LOGIN.Services.Interfaces
{
    public interface IStateService
    {
        Task<ResponseDto<List<StateDto>>> GetAllStatesAsync();
        Task<ResponseDto<StateDto>> GetStateByIdAsync(Guid id);
    }
}
