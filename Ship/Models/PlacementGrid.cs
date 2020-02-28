using System;
using System.Collections.Generic;
using System.Text;


namespace ShipModel
{
    [Serializable]
    public class PlacementGrid : Grid
    {
        public PlacementGrid() : base()
        {
        }

        // Determine if the chosen location for the ship is valid and if so, record the location.
        public List<GridSquare> TestShipLocation(Ship ship)
        {
            Console.WriteLine("Attempting to place ship '{0}' of type {1} (size {2}) at starting position {3},{4} facing {5}:", ship.Name,ship.ShipType, ship.Length, ship.PositionX, ship.PositionY, ship.Facing);

            #region Check for starting positions which are off the grid.
            // A Starting position is defined as the front of the ship.
            if (ship.PositionX < 0 || ship.PositionX > (Constants.GridWidth-1) || ship.PositionY < 0 || ship.PositionY > (Constants.GridHeight-1))
            {
                throw new ApplicationException("Ship starting position is not on the grid.");
            }
            #endregion

            #region Calculate rear square, eliminate ship if partially off grid then calculate intermediate squares.
            string offgridError = String.Format("Ship of size {0} would be partially off the edge of the grid.", ship.Length);

            Console.WriteLine("Squares occupied by ship:");
            List<GridSquare> sq = new List<GridSquare>();

            switch (ship.Facing)
            {
                case Direction.North:
                    // Check if ending square is off grid.
                    if ((ship.PositionY - (ship.Length - 1)) < 0) { throw new ApplicationException(offgridError); }     
                    // Get intermediate squares...
                    sq.AddRange(GetShipSquares(ship.PositionX, ship.PositionY, ship.Facing, ship.ShipType));
                    break;
                case Direction.South:
                    // Check if ending square is off grid.
                    if ((ship.PositionY + (ship.Length - 1)) > (Constants.GridHeight - 1)) { throw new ApplicationException(offgridError); }
                    // Get intermediate squares...
                    sq.AddRange(GetShipSquares(ship.PositionX, ship.PositionY, ship.Facing, ship.ShipType));
                    break;
                case Direction.East:
                    // Check if ending square is off grid.
                    if ((ship.PositionX - (ship.Length - 1)) < 0) { throw new ApplicationException(offgridError); }
                    // Get intermediate squares...
                    sq.AddRange(GetShipSquares(ship.PositionX, ship.PositionY, ship.Facing, ship.ShipType));
                    break;
                case Direction.West:
                    // Check if ending square is off grid.
                    if ((ship.PositionX + (ship.Length - 1)) > (Constants.GridWidth - 1)) { throw new ApplicationException(offgridError); }
                    // Get intermediate squares...
                    sq.AddRange(GetShipSquares(ship.PositionX, ship.PositionY, ship.Facing, ship.ShipType));
                    break;
            }

            //Console.WriteLine("Ending square = {0},{1}",endingX,endingY);
            #endregion

            # region Check any requested squares are not already occupied by another ship.
            // Occupied square are marked as flagged
            foreach (GridSquare square in sq)
            {
                if (square.Marked)
                {
                    throw new ApplicationException(String.Format("Square: {0},{1} is currently occupied by {2} ({3})", square.PositionX, square.PositionY, square.CurrentOccupier.Name,square.CurrentOccupier.ShipType.ToString()));
                }  
            }
            #endregion

            return sq;

        }

        // Get the squares covered by a ship of type ShipType facing facingDirection and at position x,y
        private List<GridSquare> GetShipSquares(int x, int y, Direction facingDirection, ShipType shipType)
        {
            List<GridSquare> squares = new List<GridSquare>();

            int counter = 0;
            switch (facingDirection)
            {
                case Direction.North:
                    counter = y;
                    while (counter > y-((int)shipType))
                    {
                        Console.WriteLine("  {0},{1}", x, counter);
                        squares.Add(Squares[x, counter]);
                        counter--;
                    }
                    break;
                case Direction.South:
                    counter = y;
                    while (y + ((int)shipType) > counter)
                    {
                        Console.WriteLine("  {0},{1}", x, counter);
                        squares.Add(Squares[x, counter]);
                        counter++;
                    }
                    break;
                case Direction.East:
                    counter = x;
                    while (counter > (x - (int)shipType))
                    {
                        Console.WriteLine("  {0},{1}", counter, y);
                        squares.Add(Squares[counter, y]);
                        counter--;
                    }
                    break;
                case Direction.West:
                    counter = x;
                    while (x + (int)shipType > counter)
                    {
                        Console.WriteLine("  {0},{1}", counter, y);
                        squares.Add(Squares[counter, y]);
                        counter++;
                    }
                    break;
            }

            return squares;

        }

        public void SetShipLocation(Ship ship, int desiredX, int desiredY, Direction facingDirection)
        {
            
            // Calculate the grid squares covered by the ship
            List<GridSquare> squares = GetShipSquares(desiredX, desiredY, facingDirection, ship.ShipType);

            // Mark each square as being occupied by this ship.
            int section = 1;
            foreach (GridSquare sq in squares)
            {
                sq.Marked = true;
                sq.CurrentOccupier = ship;
                sq.CurrentOccupierFacingDirection = ship.Facing;
                sq.ShipSection = section;
                section++;
            }
        }




        //// Mark a given set of squares as occupied by a given ship.
        //public void SetShipLocation(List<GridSquare> squares, Ship ship)
        //{
        //    int section = 1;

        //    foreach (GridSquare sq in squares)
        //    {
        //        sq.Marked = true;
        //        sq.CurrentOccupier = ship;
        //        sq.CurrentOccupierFacingDirection = ship.Facing;
        //        sq.ShipSection = section;
        //        section++;
        //    }
        //}
    }
}
