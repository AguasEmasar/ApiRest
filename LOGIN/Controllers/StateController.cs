using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LOGIN.Controllers
{
    [Route("api/state")]
    [ApiController]
    public class StateController : Controller
    {
        private readonly IStateService _stateServices;

        public StateController(IStateService stateServices)
        {
            _stateServices = stateServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStates()
        {
            var response = await _stateServices.GetAllStatesAsync();

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStateById(Guid id)
        {
            var response = await _stateServices.GetStateByIdAsync(id);

            if (response.Status)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
