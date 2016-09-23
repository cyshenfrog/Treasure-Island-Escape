using UnityEngine;
using System.Collections.Generic;

public class Sea : TileData
{
    public Sea(Vector2 position, TileData fromTile = null) : base(MapConstants.LandformType.Sea, position, fromTile)
    {
        this.position = position;
        this.fromTile = fromTile != null ? fromTile : this;
        center = this;
        isRunable = false;
        isConstructable = false;

        materialTypes[0] = MapConstants.LandformType.Sea;
    }
}