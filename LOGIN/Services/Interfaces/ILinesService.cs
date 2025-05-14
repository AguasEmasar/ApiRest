using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Lines;

namespace LOGIN.Services.Interfaces
{
    public interface ILinesService
    {
        Task<ResponseDto<LinesDto>> CreateLine(LinesCreateDto createDto);
        Task<ResponseDto<IEnumerable<LinesDto>>> GetAllLineAsync();
        Task<ResponseDto<LinesDto>> GetByIdLineAsync(Guid id);
        Task<ResponseDto<LinesDto>> UpdateLine(Guid id, LinesCreateDto updateDto);
    }
}
