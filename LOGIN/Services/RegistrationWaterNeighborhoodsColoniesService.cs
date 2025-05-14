using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.RegistrationWater;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RegistrationWaterNeighborhoodsColoniesService : IRegistrationWaterNeighborhoodsColoniesService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RegistrationWaterNeighborhoodsColoniesService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddRangeAsync(IEnumerable<RegistrationWaterNeighborhoodsColoniesDto> dtos)
    {
        var entities = _mapper.Map<IEnumerable<RegistrationWaterNeighborhoodsColoniesEntity>>(dtos);
        foreach (var entity in entities)
        {
            entity.Id = Guid.NewGuid();
        }
        await _context.RegistrationWaterNeighborhoodsColonies.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<ResponseDto<IEnumerable<RegistrationWaterNeighborhoodsColoniesDto>>> GetByNeighborhoodsColoniesAsync(IEnumerable<Guid> neighborhoodColoniesIds)
    {
        var entities = await _context.RegistrationWaterNeighborhoodsColonies
            .Where(rwnc => neighborhoodColoniesIds.Contains(rwnc.NeighborhoodColoniesId))
            .Include(rw => rw.RegistrationWater)
            .ToListAsync();

        var dtos = _mapper.Map<IEnumerable<RegistrationWaterNeighborhoodsColoniesDto>>(entities);

        return new ResponseDto<IEnumerable<RegistrationWaterNeighborhoodsColoniesDto>>
        {
            Status = true,
            StatusCode = 200,
            Message = "Se obtuvieron los registros correctamente",
            Data = dtos
        };
    }

    public async Task<RegistrationWaterNeighborhoodsColoniesDto> GetByIdAsync(Guid id)
    {
        var entity = await _context.RegistrationWaterNeighborhoodsColonies
            .FirstOrDefaultAsync(rwnc => rwnc.Id == id);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<RegistrationWaterNeighborhoodsColoniesDto>(entity);
    }
}