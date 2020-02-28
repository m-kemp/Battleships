using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipModel
{
    /// <summary>
    /// Keeps track of the ships that make up a fleet
    /// </summary>
    public class Fleet
    {
        #region Fleet property definitions
        private Ship aircraftCarrier = null;
        public Ship AircraftCarrier
        {
            get
            {
                if (aircraftCarrier == null)
                {
                    return null;
                }
                else
                {
                    return aircraftCarrier;
                }
            }
            set
            {
                if (aircraftCarrier == null)
                {
                    aircraftCarrier = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed AircraftCarrier #1.");
                }
            }
        }

        private Ship destroyer1 = null;
        public Ship Destroyer1
        {
            get
            {
                if (destroyer1 == null)
                {
                    return null;
                }
                else
                {
                    return destroyer1;
                }
            }
            set
            {
                if (destroyer1 == null)
                {
                    destroyer1 = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Destoyer #1.");
                }
            }
        }

        private Ship destroyer2 = null;
        public Ship Destroyer2
        {
            get
            {
                if (destroyer2 == null)
                {
                    return null;
                }
                else
                {
                    return destroyer2;
                }
            }
            set
            {
                if (destroyer2 == null)
                {
                    destroyer2 = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Destoyer #2.");
                }
            }
        }

        private Ship frigate1 = null;
        public Ship Frigate1
        {
            get
            {
                if (frigate1 == null)
                {
                    return null;
                }
                else
                {
                    return frigate1;
                }
            }
            set
            {
                if (frigate1 == null)
                {
                    frigate1 = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Frigate #1.");
                }
            }
        }

        private Ship frigate2 = null;
        public Ship Frigate2
        {
            get
            {
                if (frigate2 == null)
                {
                    return null;
                }
                else
                {
                    return frigate2;
                }
            }
            set
            {
                if (frigate2 == null)
                {
                    frigate2 = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Frigate #2.");
                }
            }
        }

        private Ship frigate3 = null;
        public Ship Frigate3
        {
            get
            {
                if (frigate3 == null)
                {
                    return null;
                }
                else
                {
                    return frigate3;
                }
            }
            set
            {
                if (frigate3 == null)
                {
                    frigate3 = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Frigate #3.");
                }
            }
        }

        private Ship submarine = null;
        public Ship Submarine
        {
            get
            {
                if (submarine == null)
                {
                    return null;
                }
                else
                {
                    return submarine;
                }
            }
            set
            {
                if (submarine == null)
                {
                    submarine = value;
                }
                else
                {
                    throw new ArgumentException("You have already placed Submarine #1.");
                }
            }
        }
        #endregion
    }
}
