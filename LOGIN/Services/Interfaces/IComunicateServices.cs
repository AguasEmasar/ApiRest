using LOGIN.Dtos;
using LOGIN.Dtos.Communicates;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;

namespace LOGIN.Services.Interfaces
{
    public interface IComunicateServices
    {
        Task<ResponseDto<CommunicateDto>> CreateCommunicate(CreateCommunicateDto model);
        Task<ResponseDto<CommunicateDto>> DeleteCommunicate(Guid id);
        Task<ResponseDto<List<CommunicateDto>>> GetAllCommunicates();
        Task<ResponseDto<CommunicateDto>> GetCommunicateById(Guid id);
        Task<ResponseDto<CommunicateDto>> UpdateCommunicate(CommunicateDto model);
    }
}
