using System.Threading.Tasks;

namespace WebApiBattleShip.Models
{
    public class Board
    {
        public BoardGrid BoardGrid { get; set; }

        public Board()
        {
            this.BoardGrid = new BoardGrid();
        }

        public async Task<bool> InitalizeBoard()
        {
            return await BoardGrid.Initialize();
        }

    }
}
