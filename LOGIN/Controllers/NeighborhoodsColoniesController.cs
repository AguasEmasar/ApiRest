using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOGIN.Controllers
{
    [ApiController]
    [Route("api/neighborhood-colony")]
    public class NeighborhoodsColoniesController : ControllerBase
    {
        private readonly INeighborhoodsColoniesService _neighborhoodsColoniesService;

        public NeighborhoodsColoniesController(INeighborhoodsColoniesService neighborhoodsColoniesService)
        {
            _neighborhoodsColoniesService = neighborhoodsColoniesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<NeighborhoodsColoniesDto>>> GetById(Guid id)
        {
            var result = await _neighborhoodsColoniesService.GetByIdNCAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("block/{blockId}")]
        public async Task<ActionResult<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>>> GetByBlockId(Guid blockId)
        {
            var result = await _neighborhoodsColoniesService.GetByBlockIdAsync(blockId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<NeighborhoodsColoniesDto>>>> GetAll()
        {
            var result = await _neighborhoodsColoniesService.GetAllNCAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<NeighborhoodsColoniesDto>>> Create(NeighborhoodsColoniesCreateDto createDto)
        {
            var result = await _neighborhoodsColoniesService.CreateNC(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<NeighborhoodsColoniesDto>>> Update(Guid id, NeighborhoodsColoniesCreateDto updateDto)
        {
            var result = await _neighborhoodsColoniesService.UpdateNC(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}