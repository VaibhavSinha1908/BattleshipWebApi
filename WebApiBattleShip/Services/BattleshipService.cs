using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Enums;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Models.RequestModels;
using WebApiBattleShip.Services.Interfaces;


namespace WebApiBattleShip.Services
{
    public class BattleshipService : IBattleshipService
    {


        private readonly ILogger<BattleshipService> logger;
        private BattleshipBoardGame battleshipBoardGame;
        private readonly IServiceHelper serviceHelper;
        private readonly Response response;

        //Create a BattleshipGame
        public BattleshipService(ILogger<BattleshipService> logger, BattleshipBoardGame battleshipBoardGame,
            IServiceHelper serviceHelper, Response response)
        {

            this.logger = logger;
            this.battleshipBoardGame = battleshipBoardGame;
            this.serviceHelper = serviceHelper;
            this.response = response;
        }


        //Create a 10x10 board.

        public async Task<bool> CreateBoard()
        {
            var board = new Board();
            bool result;
            try
            {
                result = await board.InitalizeBoard();
                logger.LogInformation("Board initialized");
                this.battleshipBoardGame.Board = board;
                response.Message = ResponseMessages.BOARD_CREATED;
            }
            catch (System.Exception ex)
            {
                throw ex;

            }
            return result;
        }


        //Poisition a ship
        public async Task<bool> AddShip(ShipAddRequest request)
        {
            bool result = false;
            var board = battleshipBoardGame.Board;

            if (board == null)
            {
                response.Message = ResponseMessages.BOARD_NOT_CREATED;
                return result;
            }


            var serializeRequest = JsonConvert.SerializeObject(request);

            //Sanitize the request.
            if (await serviceHelper.IsRequestValid(request))
            {
                //check Orientation
                if (request.Orientation == nameof(Orientation.Vertical).ToLower())
                {
                    result = await serviceHelper.AddShipVertically(request, board);
                }
                else
                {
                    if (request.Orientation == nameof(Orientation.Horizontal).ToLower())
                    {
                        result = await serviceHelper.AddShipHorizontally(request, board);
                    }
                }
            }
            if (!result)
            {
                logger.LogInformation($"Invalid request for Ship: {serializeRequest}");

            }
            return result;
        }


        //Process an attack on the board.
        public async Task<bool> AttackShip(AttackRequest request)
        {
            bool result = false;
            var board = battleshipBoardGame.Board;

            if (board == null)
            {
                response.Message = ResponseMessages.BOARD_NOT_CREATED;
                return result;
            }

            var validRequest = await serviceHelper.IsRequestValid(request);
            //Check if the request is valid.
            if (validRequest)
            {
                int.TryParse(request.VerticalPosition, out int verticalPos);
                int.TryParse(request.HorizontalPosition, out int horizontalPos);

                //find if any ships have these coordinates.
                IEnumerable<BattleShip> ships = battleshipBoardGame.Ships.Where(x => x.ShipPosition.Where(p => p.VIndex == verticalPos && p.HIndex == horizontalPos).Any());

                //If any ship exists with those coordinates.
                if (ships.Any())
                {
                    //Set hit status.
                    ships.ToArray()[0].ShipPosition.FirstOrDefault(x => x.HIndex == horizontalPos && x.VIndex == verticalPos).Hit = true;

                    //set repsonse to True.
                    response.Message = ResponseMessages.ATTACK_SUCCESSFUL;
                    result = true;
                    var shipUnderAttack = ships.ToArray()[0];

                    //check total number of hits on this ship.
                    var count = ships.ToArray()[0].ShipPosition.Where(x => x.Hit == true).Count();
                    var ship = ships.ToArray()[0];
                    if (count == ships.ToArray()[0].LengthOfShip)
                    {
                        response.Message = ResponseMessages.ATTACK_SUNK_SHIP;
                        result = true;

                        //remove the ship
                        battleshipBoardGame.Ships.Remove(ship);

                        //Reset the grid section.
                        foreach (var cell in ship.ShipPosition)
                        {
                            board.BoardGrid.Grid[cell.VIndex][cell.HIndex].OccupiedStatus = OccupiedStatus.NotOccupied;
                            board.BoardGrid.Grid[cell.VIndex][cell.HIndex].Hit = false;
                        }
                    }

                }
            }
            if (validRequest && !result)
            {
                response.Message = ResponseMessages.ATTACK_MISS;
            }

            return result;
        }


    }
}
