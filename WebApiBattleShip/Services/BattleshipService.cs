using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Enums;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Services.Interfaces;

namespace WebApiBattleShip.Services
{
    public class BattleshipService : IBattleshipService
    {


        private readonly ILogger<BattleshipService> logger;
        private BattleshipBoardGame battleshipBoardGame;

        //Create a BattleshipGame
        public BattleshipService(ILogger<BattleshipService> logger, BattleshipBoardGame battleshipBoardGame)
        {

            this.logger = logger;
            this.battleshipBoardGame = battleshipBoardGame;
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

            var serializeRequest = JsonConvert.SerializeObject(request);

            //Sanitize the request.
            if (IsRequestValid(request))
            {


                //check Orientation
                if (request.Orientation == nameof(Orientation.Vertical).ToLower())
                {
                    result = await AddShipVertically(request, board);
                }
                else
                {
                    if (request.Orientation == nameof(Orientation.Horizontal).ToLower())
                    {
                        result = await AddShipHorizontally(request, board);
                    }
                }
            }

            logger.LogInformation($"Invalid request for Ship: {serializeRequest}");
            return result;

        }

        private async Task<bool> AddShipHorizontally(ShipAddRequest request, Board board)
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

        private async Task<bool> AddShipVertically(ShipAddRequest request, Board board)
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
                return true;
            }

            logger.LogInformation($"Ship : {serializeRequest} was not added!");
            return false;

        }

        private async Task<bool> IsShipPlacementVerticallyPossible(ShipAddRequest request, List<List<Cell>> grid)
        {
            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);

            for (int i = 0; i < Length; i++)
            {
                if (verticalPos > 10)
                {
                    return false;
                }

                if (grid[verticalPos][horizontalPos].OccupiedStatus == OccupiedStatus.Occupied)
                    return false;

                //increment vertical index.
                verticalPos++;
            }

            //Good to add the ship.
            return true;
        }


        private async Task<bool> IsShipPlacementHorizontallyPossible(ShipAddRequest request, List<List<Cell>> grid)
        {
            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);


            for (int i = 0; i < Length; i++)
            {
                if (horizontalPos > 10)
                {
                    return false;
                }

                if (grid[verticalPos][horizontalPos].OccupiedStatus == OccupiedStatus.Occupied)
                    return false;

                //increment vertical index.
                horizontalPos++;
            }

            //Good to add the ship.
            return true;
        }


        private bool IsRequestValid(ShipAddRequest request)
        {
            int h = -1, v = -1, l = -1;
            var serializeRequest = JsonConvert.SerializeObject(request);


            //check if the coordinates are valid
            if (!int.TryParse(request.VerticalStartingPoint, out v))
                return false;
            else if (!int.TryParse(request.HorizontalStartingPoint, out h))
                return false;
            else
            {
                if (v > 10 || v < 0)
                    return false;
                if (h > 10 || h < 0)
                    return false;

            }
            if (!int.TryParse(request.Length, out l))
                return false;
            if (string.IsNullOrEmpty(request.Orientation))
                return false;
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
            return false;
        }


        //Process an attack on the board.

        public bool AttackShip()
        {
            throw new System.NotImplementedException();
        }


    }
}
