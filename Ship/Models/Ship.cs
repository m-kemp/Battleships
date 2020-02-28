using System;
using System.Collections.Generic;
using System.Text;


namespace ShipModel
{
    public class Ship
    {
        public ShipType ShipType { get; set; }

        Status _status;
        public Status Status { get { return CalculateOperationalStatus(); } set { _status = value; } }

        public string Name { get; set; }

        public Direction Facing { get; set; }

        public bool[] Damage { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int Length
        {
            get { return (int)ShipType; }
        }
  
        public Ship(string name, ShipType shipType, int x, int y, Direction facing)
        {
            Name = name;
            ShipType = shipType;
            PositionX = x;
            PositionY = y;
            Facing = facing;

            // Set the damage counter to zero.
            Damage = new bool[(int)shipType];
            for (int i = 0; i < Damage.Length; i++)
            {
                Damage[i] = false;
            }
        }

        private Status CalculateOperationalStatus()
        {
            // If any one section of the ship is not damaged the whole ship is still operational.
            foreach(bool d in Damage)
            {
                if (!d) return Status.Operational;
            }
            // All sections are damaged.
            Console.WriteLine("The {0} {1} has been destroyed and is no longer operational.", ShipType,Name);
            return Status.Destroyed;     
        }

        // Record when a ship is hit.
        public void MarkHit(int section) // sections are numbered 1,2,3 etc. from front of ship
        {
            if(Damage[section - 1] == true)
            {
                Console.WriteLine("..........but section has already been damaged. No more points awarded;");
                return;
            }
            Damage[section - 1] = true;
            // TODO: Update points.
            CalculateOperationalStatus();
        }

    }

}
