using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiBattleShip.Models;
using WebApiBattleShip.Models.Request;
using WebApiBattleShip.Models.RequestModels;

namespace WebApiBattleShip.Services.Interfaces
{
    public interface IServiceHelper
    {
        Task<bool> AddShipHorizontally(ShipAddRequest request, Board board);
        Task<bool> AddShipVertically(ShipAddRequest request, Board board);
        Task<bool> IsShipPlacementVerticallyPossible(ShipAddRequest request, List<List<Cell>> grid);
        Task<bool> IsShipPlacementHorizontallyPossible(ShipAddRequest request, List<List<Cell>> grid);
        Task<bool> IsRequestValid(ShipAddRequest request);
        Task<bool> IsRequestValid(AttackRequest request);
    }
}
