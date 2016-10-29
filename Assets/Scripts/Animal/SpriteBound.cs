﻿using UnityEngine;
using System.Collections.Generic;

public class SpriteBound {

    public float Height {
        private set {
            if (value > 1) Height = 1;
            else if (value < 0) Height = 0;
            else Height = value;
        }
        get { return Height; }
    }
    public Vector2 Extend { private set; get; }

    private Sprite Render;
    private Vector2[] vertex = new Vector2[4];

    public SpriteBound(Sprite render, float height = 1) {
        Height = height;
        Render = render;
        Extend = Render.bounds.extents;

        vertex[0] = new Vector2(Extend.x, (Height - 0.5f) * 2 * Extend.y);
        vertex[1] = new Vector2(-Extend.x, (Height - 0.5f) * 2 * Extend.y);
        vertex[1] = new Vector2(-Extend.x, -(Height - 0.5f) * 2 * Extend.y);
        vertex[1] = new Vector2(Extend.x, (Height - 0.5f) * 2 * Extend.y);
    }

    /// <summary>
    /// 0 for quadrant 1, 1 for quadrant 2 ...
    /// </summary>
    /// <param name="spriteCenter"></param>
    /// <param name="quadrant"></param>
    /// <returns></returns>
    public Vector2 GetVertex(Vector2 spriteCenter, int quadrant) {
        return spriteCenter + vertex[quadrant];
    }

    /// <summary>
    /// Return IsRunable for a sprite
    /// </summary>
    /// <param name="spriteCenter"></param>
    /// <returns></returns>
    public bool CheckTile(Vector2 spriteCenter, Vector2 delta) {
        Vector2 point = GetVertex(spriteCenter + delta, 0);
        for (int i = 0; i < 4; i++) {
            Vector2 next = GetVertex(spriteCenter + delta, (i + 1) % 4);
            while (point.x != next.x || point.y != next.y) {
                if (!GroundController.GetTileDataByWorldPosition(point).IsRunable)
                    return false;

                Vector2.MoveTowards(
                    point, 
                    next, 
                    i == 1 || i == 3 ? GroundController.StaticWorldHeightInWC : GroundController.StaticWorldWidthInWC);
            }
            point = GetVertex(spriteCenter, i + 1);
        }       
        return true;
    }

    public void RegisterTile(Vector2 spriteCenter) {
        if (CheckTile(spriteCenter, Vector2.zero)) {
        }
    }
}
