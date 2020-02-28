using System;
using System.Collections.Generic;
using System.Text;


namespace ShipModel
{
    [Serializable]
    public class GridSquare
    {
        //public int Id { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        // Set true if:
        // 1. Square occupied by a ship
        // 2. Player has already fired against enemy at this grid square.
        public bool Marked { get; set; }

        // Used for recording the hit/miss of a shoot on the tracking board.
        public bool HitPeg { get; set; }

        // which section of the ship (from the front) occupies this square
        // numbered 1 to X.
        public int ShipSection { get; set; }

        public Ship CurrentOccupier { get; set; }

        public Direction CurrentOccupierFacingDirection { get; set; }
    }
}
