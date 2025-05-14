using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using LOGIN.Dtos;

public interface IRegistrationWaterService
{
    Task<ResponseDto<RegistrationWaterDto>> CreateAsync(RegistrationWaterCreateDto createDto);
    Task<ResponseDto<bool>> DeleteAsync(Guid id);
    Task<ResponseDto<IEnumerable<RegistrationWaterDto>>> GetAllAsync();
    Task<ResponseDto<RegistrationWaterDto>> GetByIdAsync(Guid id);
    Task<ResponseDto<RegistrationWaterDto>> UpdateAsync(Guid id, RegistrationWaterCreateDto updateDto);
}