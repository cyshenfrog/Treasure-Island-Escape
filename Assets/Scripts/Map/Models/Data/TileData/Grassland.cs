using UnityEngine;
using System.Collections.Generic;

public class Grassland : TileData
{
    public Grassland(Vector2 position, TileData fromTile = null) : base(MapConstants.LandformType.Sea, position, fromTile)
    {
        this.position = position;
        this.fromTile = fromTile != null ? fromTile : this;
        center = this;
        isRunable = true;
        isConstructable = true;

        materialTypes[0] = MapConstants.LandformType.Grassland;
    }
}