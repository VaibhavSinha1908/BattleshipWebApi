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



        /// <summary>
        /// Creates a new [10x10] board with grid index starting 0-9. 
        /// For instance, the starting index of the grid cell is [0,0] and the last cell's index is [9,9]
        /// </summary>
        /// <remarks>
        /// sample request:
        /// Post
        /// {
        /// }
        /// </remarks>
        /// <returns>A new [10x10] board</returns>
        /// <response code = "201">creates [10x10] board in memory.</response>
        /// <response code = "500">if exception</response>
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



        /// <summary>
        /// Add a ship to the board with vertical and horizontal indexes ranging from 0-9 and length ranging from 1-10.
        /// </summary>
        /// <remarks>
        /// sample request:
        /// Post /AddShip
        /// {
        ///     "verticalHeadPosition":6,
        ///     "horizontalHeadPosition": 6,
        ///     "size": 2,
        ///     "orientation": "vertical"
        /// }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>A ship of given length added at the desired location.</returns>
        /// <response code = "200">The ship has been added.</response>
        /// <response code = "400">The ship cannot be added as it coincides with another ship.</response>
        /// <response code = "400">The ship cannot be added as coordinates are out of bounds.</response>
        /// <response code = "400">The ship cannot be added.</response>
        /// <response code = "500">Internal server error. (in case of exception)</response>
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



        /// <summary>
        /// Attacks the ship at a coordinate (with coordinate ranging from [0-9]).
        /// </summary>
        /// <remarks>
        /// sample request:
        /// Post /AttackShip
        /// {
        ///     "verticalPosition":1
        ///     "horizontalPosition": 2,
        /// }
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Returns whether the attack hits a ship or not.</returns>
        /// <response code = "200">The attack hit a ship.</response>
        /// <response code = "400">The request format was invalid.</response>
        /// <response code = "500">Internal server error. (in case of exception)</response>
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




    }
}
