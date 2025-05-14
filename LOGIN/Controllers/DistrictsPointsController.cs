using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Districts;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOGIN.Controllers
{
    [ApiController]
    [Route("api/districts-points")]
    public class DistrictsPointsController : ControllerBase
    {
        private readonly IDistrictsPointsService _districtsPointsService;

        public DistrictsPointsController(IDistrictsPointsService districtsPointsService)
        {
            _districtsPointsService = districtsPointsService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<DistrictsPointsDto>>> GetByIdDistrictsPoints(Guid id)
        {
            var result = await _districtsPointsService.GetByIdDistrictsPointsAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<DistrictsPointsDto>>>> GetAllDistrictsPoints()
        {
            var result = await _districtsPointsService.GetAllDistrictsPointsAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DistrictsPointsDto>>> Create(DistrictsPointsCreateDto createDto)
        {
            var result = await _districtsPointsService.CreateDistrictsPoints(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<DistrictsPointsDto>>> Update(Guid id, DistrictsPointsCreateDto updateDto)
        {
            var result = await _districtsPointsService.UpdateDistrictsPoints(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("byNeighborhoodsColonies/{neighborhoodsColoniesId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<DistrictsPointsDto>>>> GetByNeighborhoodsColoniesId(Guid neighborhoodsColoniesId)
        {
            var result = await _districtsPointsService.GetByNeighborhoodsColoniesIdAsync(neighborhoodsColoniesId);
            return StatusCode(result.StatusCode, result);
        }
    }
}