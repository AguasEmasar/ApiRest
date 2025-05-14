using LOGIN.Dtos;
using LOGIN.Dtos.ReportDto;

namespace LOGIN.Services.Interfaces
{
    public interface IReportService
    {
        Task<ResponseDto<ReportDto>> ChangeStateReportAsync(Guid id, Guid stateId);
        Task<ResponseDto<ReportDto>> CreateReportAsync(CreateReportDto model);
        Task<ResponseDto<ReportDto>> DeleteReportAsync(Guid id);
        Task<ResponseDto<List<ReportDto>>> GetAllReportsAsync();
        Task<string> GetImageUrl(string publicId);
        Task<ResponseDto<ReportDto>> GetReportByIdAsync(Guid id);
        Task<ResponseDto<ReportDto>> UpdateReportAsync(UpdateReportDto model);
    }
}
