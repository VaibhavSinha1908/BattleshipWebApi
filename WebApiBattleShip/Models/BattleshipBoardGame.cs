using System.Collections.Generic;

namespace WebApiBattleShip.Models
{
    public class BattleshipBoardGame
    {
        //Board
        public Board Board { get; set; }


        //List of ships.
        public List<BattleShip> Ships { get; set; }

        public BattleshipBoardGame()
        {
            Ships = new List<BattleShip>();
        }
    }
}
