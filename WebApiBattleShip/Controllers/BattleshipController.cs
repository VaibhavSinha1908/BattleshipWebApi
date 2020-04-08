using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Models.RequestModels;
using WebApiBattleShip.Services.Interfaces;

namespace WebApiBattleShip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly ILogger<BattleshipController> logger;
        private readonly IBattleshipService battleshipService;
        private readonly Response response;


        //DI to access the service actions.
        public BattleshipController(ILogger<BattleshipController> logger, IBattleshipService battleshipService, Response response)
        {
            this.logger = logger;
            this.battleshipService = battleshipService;
            this.response = response;
        }


        [HttpPost]
        [Route("AddShip")]
        public async Task<IActionResult> Post([FromBody] ShipAddRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await battleshipService.AddShip(request);
                    if (result)
                        return Ok(response.Message);
                    else
                        return BadRequest(response.Message);
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }

        }


        [HttpPost]
        [Route("AttackShip")]
        public async Task<IActionResult> AttackShip([FromBody] AttackRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var result = await battleshipService.AttackShip(request);
                    if (result)
                        return Ok(response.Message);
                    else
                        return BadRequest(response.Message);

                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]
        [Route("CreateBoard")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var result = await battleshipService.CreateBoard();
                if (result)
                {
                    return StatusCode(201, response.Message);
                }
                else
                    return BadRequest(response.Message);

            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
