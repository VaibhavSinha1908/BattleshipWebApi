using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Services.Interfaces;

namespace WebApiBattleShip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly ILogger<BattleshipController> logger;
        private readonly IBattleshipService battleshipService;


        //DI to access the service actions.
        public BattleshipController(ILogger<BattleshipController> logger, IBattleshipService battleshipService)
        {
            this.logger = logger;
            this.battleshipService = battleshipService;
        }


        public string Get()
        {
            return "It is working";
        }


        [HttpPost]
        [Route("AddShip")]
        public async Task<IActionResult> Post([FromBody] ShipAddRequest request)
        {
            try
            {
                var result = await battleshipService.AddShip(request);
                if (result)
                    return Ok("Ship Added");
                else
                    return BadRequest("AddShip() failed.");

            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);
                BadRequest(ex);
            }
            return BadRequest("AddShip() failed.");
        }



        [HttpPost]
        [Route("createboard")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var response = await battleshipService.CreateBoard();
                if (response)
                {
                    return StatusCode(201, "10x10 Board Created");
                }
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex.Message, ex.StackTrace);

            }
            return StatusCode(500);

        }

    }
}
