using LOGIN.Dtos;
using LOGIN.Dtos.ScheduleDtos.Blocks;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LOGIN.Controllers
{
    [ApiController]
    [Route("api/block")]
    public class BlocksController : ControllerBase
    {
        private readonly IBlocksService _blocksService;

        public BlocksController(IBlocksService blocksService)
        {
            _blocksService = blocksService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<BlockDto>>> GetByIdBlockasync(Guid id)
        {
            var result = await _blocksService.GetByIdBloqueAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto<IEnumerable<BlockDto>>>> GetAllBlockasync()
        {
            var result = await _blocksService.GetAllBloqueAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto<BlockDto>>> CreateBlock(BlockCreateDto createDto)
        {
            var result = await _blocksService.CreateBloque(createDto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseDto<BlockDto>>> UpdateBlock(Guid id, BlockCreateDto updateDto)
        {
            var result = await _blocksService.UpdateAsync(id, updateDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}