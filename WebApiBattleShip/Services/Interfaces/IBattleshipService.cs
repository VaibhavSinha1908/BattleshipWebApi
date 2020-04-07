using System.Threading.Tasks;

namespace WebApiBattleShip.Services.Interfaces
{
    public interface IBattleshipService
    {
        Task<bool> CreateBoard();

        Task<bool> AddShip(Models.Request.ShipAddRequest request);

        bool AttackShip();
    }
}
