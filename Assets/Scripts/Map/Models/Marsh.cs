using UnityEngine;
using System.Collections.Generic;

public class Marsh : TileData
{
    public Marsh(Vector2 position, TileData fromTile = null)
    {
        this.position = position;
        this.fromTile = fromTile != null ? fromTile : this;
        center = this;
        isRunable = true;
        isConstructable = true;

        materialTypes[0] = MapConstants.LandformType.Marsh;
    }
}