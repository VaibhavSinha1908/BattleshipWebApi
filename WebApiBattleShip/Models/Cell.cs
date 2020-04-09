namespace WebApiBattleShip.Models
{
    public class Cell
    {
        //index from 1 - 10.
        public int HIndex { get; set; }
        public int VIndex { get; set; }
        public OccupiedStatus OccupiedStatus { get; set; }
        public bool Hit { get; set; }   //false to beging with.

        public Cell()
        {
            this.OccupiedStatus = OccupiedStatus.NotOccupied;
        }

    }
}