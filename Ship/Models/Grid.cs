using System;
using System.Collections.Generic;
using System.Text;


namespace ShipModel
{
    [Serializable]
    public class Grid
    {
        //public Team Team { get; set; }

        // The main grid which contains and array of GridSquares.
        public GridSquare[,] Squares = new GridSquare[Constants.GridWidth, Constants.GridHeight];

        public Grid()
        {
            for (int y = 0; y < Constants.GridHeight; y++)
            {
                for (int x = 0; x < Constants.GridWidth; x++)
                {
                    GridSquare s = new GridSquare() { PositionX = x, PositionY = y, Marked = false, HitPeg = false };
                    Squares[x,y] = s;
                }
            }
        }
    }
}
