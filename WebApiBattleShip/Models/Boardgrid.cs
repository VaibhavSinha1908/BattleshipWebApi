using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApiBattleShip.Models
{

    public class BoardGrid
    {

        public List<List<Cell>> Grid { get; set; }

        public BoardGrid()
        {

            Grid = new List<List<Cell>>();

        }

        public async Task<bool> Initialize()
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    var cellList = new List<Cell>();

                    for (int j = 0; j < 10; j++)
                    {
                        cellList.Add(AddCell(i, j));
                    }

                    //Add the list to the board
                    Grid.Add(cellList);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Cell AddCell(int i, int j)
        {
            var cell = new Cell
            {
                VIndex = i,
                HIndex = j
            };
            return cell;
        }

    }
}
