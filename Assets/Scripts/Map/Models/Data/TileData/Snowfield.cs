using UnityEngine;
using System.Collections.Generic;

public class Snowfield : TileData
{
    public Snowfield(Vector2 position, TileData fromTile = null) : base(MapConstants.LandformType.Sea, position, fromTile)
    {
        this.position = position;
        this.fromTile = fromTile != null ? fromTile : this;
        center = this;
        isRunable = true;
        isConstructable = true;

        materialTypes[0] = MapConstants.LandformType.Snowfield;
    }
}