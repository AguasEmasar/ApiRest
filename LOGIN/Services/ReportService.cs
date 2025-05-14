using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using LOGIN.Dtos.ReportDto;
using CloudinaryDotNet.Actions;

namespace LOGIN.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<ReportService> logger)
        {
            _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            _cloudinary.Api.Secure = true;

            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ResponseDto<ReportDto>> CreateReportAsync(CreateReportDto model)
        {
            try
            {
                var publicIds = new List<string>();
                var urls = new List<string>();

                foreach (var file in model.Files)
                {
                    using var stream = file.OpenReadStream();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    // Subir imagen a Cloudinary
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    publicIds.Add(uploadResult.PublicId);
                    urls.Add(uploadResult.SecureUrl.ToString());
                }

                // Buscar el estado del reporte
                var defaultState = await _dbContext.States.FirstOrDefaultAsync(x => x.Name == "no asignado");

                var reportEntity = new ReportEntity
                {
                    PublicIds = publicIds,
                    Urls = urls,
                    Key = model.Key,
                    Name = model.Name,
                    DNI = model.DNI,
                    Cellphone = model.Cellphone,
                    Date = model.Date,
                    Report = model.Report,
                    Direction = model.Direction,
                    Observation = model.Observation,
                    StateId = defaultState?.Id ?? Guid.NewGuid()
                };

                _dbContext.Reports.Add(reportEntity);
                await _dbContext.SaveChangesAsync();

                var reportDto = _mapper.Map<ReportDto>(reportEntity);

                _logger.LogInformation("Reporte creado exitosamente con ID: {ReportId}", reportEntity.Id);
                return new ResponseDto<ReportDto>
                {
                    Status = true,
                    StatusCode = 201,
                    Message = "Reporte creado con exito",
                    Data = reportDto
                };
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                _logger.LogError(dbEx, "Error al guardar el reporte en la base de datos: {ErrorMessage}", innerException);

                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "An error occurred while saving the entity changes. Details: " + innerException,
                    Data = null
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error inesperado al crear el reporte: {ErrorMessage}", e.Message);
                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = e.Message,
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<List<ReportDto>>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _dbContext.Reports.Include(r => r.State).ToListAsync();
                var reportsDto = _mapper.Map<List<ReportDto>>(reports);

                _logger.LogInformation("Se encontraron {Count} reportes.", reports.Count);
                return new ResponseDto<List<ReportDto>>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Lista de reportes",
                    Data = reportsDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los reportes: {ErrorMessage}", ex.Message);
                return new ResponseDto<List<ReportDto>>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al obtener los reportes.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<ReportDto>> GetReportByIdAsync(Guid id)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.Include(r => r.State).FirstOrDefaultAsync(x => x.Id == id);

                if (reportEntity == null)
                {
                    return new ResponseDto<ReportDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Reporte no encontrado"
                    };
                }

                var reportDto = _mapper.Map<ReportDto>(reportEntity);

                _logger.LogInformation("Reporte encontrado con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Reporte encontrado",
                    Data = reportDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar el reporte con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al buscar el reporte.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<ReportDto>> UpdateReportAsync(UpdateReportDto model)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (reportEntity == null)
                {
                    return new ResponseDto<ReportDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Reporte no encontrado"
                    };
                }

                reportEntity.Key = model.Key;
                reportEntity.Name = model.Name;
                reportEntity.DNI = model.DNI;
                reportEntity.Cellphone = model.Cellphone;
                reportEntity.Report = model.Report;
                reportEntity.Direction = model.Direction;
                reportEntity.Observation = model.Observation;
                reportEntity.StateId = model.StateId;

                if (model.Files != null)
                {
                    var publicIds = new List<string>();
                    var urls = new List<string>();

                    foreach (var file in model.Files)
                    {
                        using var stream = file.OpenReadStream();
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(file.FileName, stream),
                            Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                        };

                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        publicIds.Add(uploadResult.PublicId);
                        urls.Add(uploadResult.SecureUrl.ToString());
                    }

                    reportEntity.PublicIds = publicIds;
                    reportEntity.Urls = urls;
                }

                await _dbContext.SaveChangesAsync();

                var reportDto = _mapper.Map<ReportDto>(reportEntity);

                _logger.LogInformation("Reporte actualizado exitosamente con ID: {ReportId}", model.Id);
                return new ResponseDto<ReportDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Reporte actualizado con exito",
                    Data = reportDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al actualizar el reporte.",
                    Data = null
                };
            }
        }

        public async Task<ResponseDto<ReportDto>> DeleteReportAsync(Guid id)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.FirstOrDefaultAsync(x => x.Id == id);

                if (reportEntity == null)
                {
                    return new ResponseDto<ReportDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Reporte no encontrado"
                    };
                }

                _dbContext.Reports.Remove(reportEntity);
                await _dbContext.SaveChangesAsync();

                var reportDto = _mapper.Map<ReportDto>(reportEntity);

                _logger.LogInformation("Reporte eliminado exitosamente con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Reporte eliminado con exito",
                    Data = reportDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el reporte con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al eliminar el reporte.",
                    Data = null
                };
            }
        }

        public Task<string> GetImageUrl(string publicId)
        {
            return Task.FromResult(_cloudinary.Api.UrlImgUp.BuildUrl(publicId));
        }

        public async Task<ResponseDto<ReportDto>> ChangeStateReportAsync(Guid id, Guid stateId)
        {
            try
            {
                var reportEntity = await _dbContext.Reports.FirstOrDefaultAsync(x => x.Id == id);

                if (reportEntity == null)
                {
                    return new ResponseDto<ReportDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Reporte no encontrado"
                    };
                }

                var newState = await _dbContext.States.FirstOrDefaultAsync(x => x.Id == stateId);
                if (newState == null)
                {
                    return new ResponseDto<ReportDto>
                    {
                        Status = false,
                        StatusCode = 404,
                        Message = "Estado no encontrado"
                    };
                }

                reportEntity.StateId = stateId;
                await _dbContext.SaveChangesAsync();

                var reportDto = _mapper.Map<ReportDto>(reportEntity);

                _logger.LogInformation("Estado del reporte actualizado exitosamente con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = true,
                    StatusCode = 200,
                    Message = "Estado del reporte actualizado con exito",
                    Data = reportDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado del reporte con ID: {ReportId}", id);
                return new ResponseDto<ReportDto>
                {
                    Status = false,
                    StatusCode = 500,
                    Message = "Error interno del servidor al cambiar el estado del reporte.",
                    Data = null
                };
            }
        }
    }
}