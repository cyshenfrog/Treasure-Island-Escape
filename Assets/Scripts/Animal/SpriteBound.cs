using UnityEngine;
using System.Collections.Generic;

public class SpriteBound {

    public float Top { private set; get; }
    public float Bottom { private set; get; }
    public float Left { private set; get; }
    public float Right { private set; get; }

    public Vector2 Center { private set; get; }
    public Vector2 Extent { private set; get; }

    private Sprite sprite;

    public SpriteBound(SpriteRenderer spr) {
        sprite = spr.sprite;
        Top = Bottom = Left = Right = 0;
        Center = spr.bounds.center;
        Extent = spr.bounds.extents;
    }

    /// <summary>
    /// opt is a Vector4(top, bottom, left, right)
    /// </summary>
    /// <param name="spr"></param>
    /// <param name="opt"></param>
    public SpriteBound(SpriteRenderer spr, Vector4 opt) : this(spr) {
        Top = opt.x;
        Bottom = opt.y;
        Left = opt.z;
        Right = opt.w;

        handlePoint();
    }

    private void handlePoint() {

        Vector2 q1, q2, q4;
        Vector2 extents = sprite.bounds.extents;

        q1 = new Vector3(extents.x * (0.5f - Right), extents.y * (0.5f - Top)) * 2;
        q2 = new Vector3(-extents.x * (0.5f - Left), extents.y * (0.5f - Top)) * 2;
        q4 = new Vector3(extents.x * (0.5f - Right), -extents.y * (0.5f - Bottom)) * 2;

        Extent = new Vector2(q1.x - q2.x, q1.y - q4.y) / 2;
        Center += new Vector2((Left - Right) * extents.x, (Bottom - Top) * extents.y);

    }

    /// <summary>
    /// update data when spriteRenderer change
    /// </summary>
    /// <param name="spr"></param>
    /// <returns></returns>
    public SpriteBound Update(SpriteRenderer spr) {
        sprite = spr.sprite;
        Center = spr.bounds.center;
        Extent = spr.bounds.extents;
        handlePoint();
        return this;
    }


}
