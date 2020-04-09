using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Enums;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Models.RequestModels;
using WebApiBattleShip.Services.Interfaces;

namespace WebApiBattleShip.Services
{
    public class ServiceHelper : IServiceHelper
    {

        private readonly ILogger<ServiceHelper> logger;
        private BattleshipBoardGame battleshipBoardGame;
        private readonly Response response;

        public ServiceHelper(ILogger<ServiceHelper> logger, BattleshipBoardGame battleshipBoardGame, Response response)
        {

            this.logger = logger;
            this.battleshipBoardGame = battleshipBoardGame;
            this.response = response;
        }

        public async Task<bool> AddShipHorizontally(ShipAddRequest request, Board board)
        {


            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);


            var serializeRequest = JsonConvert.SerializeObject(request);

            logger.LogInformation($"Starting horizontal addition for Ship:{serializeRequest}");
            //bool result = false;

            var boardGrid = board.BoardGrid;

            //check if addition possible
            if (await IsShipPlacementHorizontallyPossible(request, boardGrid.Grid))
            {
                logger.LogInformation("Initiating Addition for Ship: " + serializeRequest);

                //Initialize the BattleShip
                var ship = new BattleShip
                {
                    LengthOfShip = Length,
                    Orientation = Orientation.Horizontal
                };


                var cells = new List<Cell>();
                //get the all the cells for the ship starting from head to end.
                for (int i = 0; i < Length; i++)
                {
                    //1. Mark the grid cells occupied.
                    boardGrid.Grid[verticalPos][horizontalPos].OccupiedStatus = OccupiedStatus.Occupied;

                    //2. Add cells to Cell list for the ship.
                    cells.Add(new Cell
                    {
                        HIndex = horizontalPos,
                        VIndex = verticalPos,
                        OccupiedStatus = OccupiedStatus.Occupied
                    });

                    //change the horizontal index
                    horizontalPos++;
                }
                //Assign the list to 
                ship.ShipPosition = cells;
                battleshipBoardGame.Ships.Add(ship);

                //Add Successful.
                logger.LogInformation($"Ship : {serializeRequest} was added successfully!");

                return true;
            }

            logger.LogInformation($"Ship : {serializeRequest} was not added!");
            return false;

        }

        public async Task<bool> IsShipPlacementHorizontallyPossible(ShipAddRequest request, List<List<Cell>> grid)
        {
            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);


            for (int i = 0; i < Length; i++)
            {
                if (horizontalPos > 9)
                {
                    response.Message = ResponseMessages.SHIP_OUTOF_BOUNDS;
                    return false;
                }

                if (grid[verticalPos][horizontalPos].OccupiedStatus == OccupiedStatus.Occupied)
                {
                    response.Message = ResponseMessages.SHIP_OVERLAPPING;
                    return false;
                }
                //increment vertical index.
                horizontalPos++;
            }

            //Good to add the ship.
            return true;
        }

        public async Task<bool> AddShipVertically(ShipAddRequest request, Board board)
        {


            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);


            var serializeRequest = JsonConvert.SerializeObject(request);
            logger.LogInformation($"Starting vertical addition for Ship:{serializeRequest}");

            //bool result = false;

            var boardGrid = board.BoardGrid;

            //check if addition possible
            if (await IsShipPlacementVerticallyPossible(request, boardGrid.Grid))
            {
                //Initialize the BattleShip
                var ship = new BattleShip
                {
                    LengthOfShip = Length,
                    Orientation = Orientation.Vertical
                };


                var cells = new List<Cell>();
                //get the all the cells for the ship starting from head to end.
                for (int i = 0; i < Length; i++)
                {
                    //1. Mark the grid cells occupied.
                    boardGrid.Grid[verticalPos][horizontalPos].OccupiedStatus = OccupiedStatus.Occupied;

                    //2. Add cells to Cell list for the ship.
                    cells.Add(new Cell
                    {
                        HIndex = horizontalPos,
                        VIndex = verticalPos,
                        OccupiedStatus = OccupiedStatus.Occupied
                    });

                    //change the vertical index
                    verticalPos++;
                }
                //Assign the list to 
                ship.ShipPosition = cells;
                battleshipBoardGame.Ships.Add(ship);

                //Add Successful.
                logger.LogInformation($"Ship : {serializeRequest} was added successfully!");
                response.Message = ResponseMessages.SHIP_ADDED;
                return true;
            }

            logger.LogInformation($"Ship : {serializeRequest} was not added!");
            return false;

        }

        public async Task<bool> IsShipPlacementVerticallyPossible(ShipAddRequest request, List<List<Cell>> grid)
        {
            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);

            for (int i = 0; i < Length; i++)
            {
                if (verticalPos > 9)
                {
                    response.Message = ResponseMessages.SHIP_OUTOF_BOUNDS;
                    return false;
                }

                if (grid[verticalPos][horizontalPos].OccupiedStatus == OccupiedStatus.Occupied)
                {
                    response.Message = ResponseMessages.SHIP_OVERLAPPING;
                    return false;
                }
                //increment vertical index.
                verticalPos++;
            }

            //Good to add the ship.
            return true;
        }

        public async Task<bool> IsRequestValid(ShipAddRequest request)
        {
            int h = -1, v = -1, l = -1;
            var serializeRequest = JsonConvert.SerializeObject(request);


            //check if the coordinates are valid
            if (!int.TryParse(request.VerticalStartingPoint, out v))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else if (!int.TryParse(request.HorizontalStartingPoint, out h))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else
            {
                if (v > 9 || v < 0)
                {
                    response.Message = ResponseMessages.INVALID_REQUEST;
                    return false;
                }
                if (h > 9 || h < 0)
                {
                    response.Message = ResponseMessages.INVALID_REQUEST;
                    return false;
                }
            }
            if (!int.TryParse(request.Length, out l))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else if (l <= 0)
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            if (string.IsNullOrEmpty(request.Orientation))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else
            {
                if (request.Orientation.ToLower() == nameof(Orientation.Horizontal).ToLower() ||
                    request.Orientation.ToLower() == nameof(Orientation.Vertical).ToLower()
                    )
                {
                    //All good...
                    logger.LogInformation($"Valid request for Ship: {serializeRequest}");
                    return true;
                }
            }

            response.Message = ResponseMessages.INVALID_REQUEST;
            return false;
        }



        public async Task<bool> IsRequestValid(AttackRequest request)
        {
            int h = -1, v = -1;
            var serializeRequest = JsonConvert.SerializeObject(request);

            //check if the coordinates are valid

            if (!int.TryParse(request.VerticalPosition, out v))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else if (!int.TryParse(request.HorizontalPosition, out h))
            {
                response.Message = ResponseMessages.INVALID_REQUEST;
                return false;
            }
            else
            {
                if (v > 10 || v < 0)
                {
                    response.Message = ResponseMessages.INVALID_REQUEST;
                    return false;
                }

                if (h > 10 || h < 0)
                {
                    response.Message = ResponseMessages.INVALID_REQUEST;
                    return false;
                }
            }

            //All good...
            logger.LogInformation($"Valid request for Attack: {serializeRequest}");
            return true;

        }
    }
}
