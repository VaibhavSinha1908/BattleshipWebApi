using System.Collections.Generic;
using WebApiBattleShip.Models.Enums;

namespace WebApiBattleShip.Models
{
    public class BattleShip
    {

        public int LengthOfShip { get; set; }
        public Orientation Orientation { get; set; }

        public List<Cell> ShipPosition { get; set; }

        public BattleShip()
        {
            ShipPosition = new List<Cell>();
        }

    }
}
