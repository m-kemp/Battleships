using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShipModel
{
    // Ship operational status.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Operational,
        Destroyed
    }

    // Directions a ship can face or travel
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    // Team names
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Team
    {
        TeamA,
        TeamB,
        Admiralty
    }

    // Result of trying to place a ship at a particular position.
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PlacementResult
    {
        Success,
        OffGrid,
        AlreadyOccupied
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShipType
    {
        AircraftCarrier = 5,
        Destroyer = 4,
        Frigate = 3,
        Submarine = 2
    }
}

