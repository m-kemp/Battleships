using System;
using System.Collections.Generic;
using System.Text;

namespace ShipModel
{
    [Serializable]
    public class MasterGameState
    {
        public PlayerState StateTeamA { get; set; }
        public PlayerState StateTeamB { get; set; }

        public MasterGameState()
        {
            StateTeamA = new PlayerState();
            StateTeamB = new PlayerState();
        }

    }

    public class PlayerState
    {
        public Nullable<bool> WinStatus { get; set; }
        public int Score { get; set; }
        public int ShotsYouHaveFired { get; set; }
        public int TotalHitsAgainstYou { get; set; }
        public PlacementGrid PlacementGrid { get; set; }
        public TrackingGrid TrackingGrid { get; set; }
        public Fleet Fleet { get; set; }
        //public int ShootThreashold { get; set; }

        public PlayerState()
        {
            PlacementGrid = new PlacementGrid();
            TrackingGrid = new TrackingGrid();
            Fleet = new Fleet();
            Score = 0;
            ShotsYouHaveFired = 0;
            TotalHitsAgainstYou = 0;
            WinStatus = null;
            //ShootThreashold = 0;
        }
    }


}
