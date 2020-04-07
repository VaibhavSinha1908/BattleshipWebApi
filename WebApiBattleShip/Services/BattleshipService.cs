using Microsoft.Extensions.Logging;
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

            //Sanitize the request.
            if (IsRequestValid(request))
            {
                //check Orientation
                if (request.Orientation == nameof(Orientation.Vertical).ToLower())
                {
                    result = await AddShipVertically(request, board);
                }
            }


            return result;

        }

        private async Task<bool> AddShipVertically(ShipAddRequest request, Board board)
        {

            int.TryParse(request.VerticalStartingPoint, out int verticalPos);
            int.TryParse(request.HorizontalStartingPoint, out int horizontalPos);
            int.TryParse(request.Length, out int Length);

            bool result = false;

            var boardGrid = board.BoardGrid;

            //check if addition possible
            //if (IsShipPlacementPossible(request, boardGrid.Grid))
            if (true)
            {

                var ship = new BattleShip();
                var cells = new List<Cell>();
                //get the all the cells for the ship starting from head to end.
                for (int i = horizontalPos; i <= Length; i++)
                {
                    //mark the grid cells occupied.
                    boardGrid.Grid[verticalPos][i].OccupiedStatus = OccupiedStatus.Occupied;

                    //Create lists to 
                    cells.Add(new Cell
                    {
                        HIndex = i,
                        VIndex = verticalPos,
                        OccupiedStatus = OccupiedStatus.Occupied
                    });
                }

                ship.ShipPosition = cells;

            }

            return result;


        }

        private bool IsRequestValid(ShipAddRequest request)
        {
            int h = -1, v = -1, l = -1;

            //check if the coordinates are valid
            if (!int.TryParse(request.VerticalStartingPoint, out v) && !int.TryParse(request.HorizontalStartingPoint, out h))
                return false;
            else
            {
                if (v > 10 || v < 1)
                    return false;
                if (h > 10 || h < 1)
                    return false;

            }
            if (!int.TryParse(request.Length, out l))
                return false;
            if (string.IsNullOrEmpty(request.Orientation))
                return false;
            else
            {
                if (request.Orientation.ToLower() != nameof(Orientation.Horizontal).ToLower() ||
                    request.Orientation.ToLower() != nameof(Orientation.Vertical).ToLower()
                    )
                {
                    return false;
                }
            }

            //All good...
            return true;
        }



        //Process an attack on the board.

        public bool AttackShip()
        {
            throw new System.NotImplementedException();
        }


    }
}
