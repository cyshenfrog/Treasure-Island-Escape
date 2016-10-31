using UnityEngine;
using System.Collections.Generic;

public class SpriteBound {

    //height
    //width
    //center
    //extent

    public float Top { private set; get; }
    public float Bottom { private set; get; }
    public float Left { private set; get; }
    public float Right { private set; get; }

    private Sprite sprite;
    private Bounds bounds;

    public SpriteBound(Sprite sprite) {
        this.sprite = sprite;
        bounds = sprite.bounds;
        Top = Bottom = Left = Right = 0;
    }

    public SpriteBound(SpriteRenderer spriteRenderer) {
        sprite = spriteRenderer.sprite;
        bounds = sprite.bounds;
        Top = Bottom = Left = Right = 0;
    }

    /// <summary>
    /// set is a Vector4(top, bottom, left, right)
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="set"></param>
    public SpriteBound(Sprite sprite, Vector4 set) : this(sprite) {
        Top = set.x;
        Bottom = set.y;
        Left = set.z;
        Right = set.w;
    }

    /// <summary>
    /// set is a Vector4(top, bottom, left, right)
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <param name="set"></param>
    public SpriteBound(SpriteRenderer spriteRenderer, Vector4 set) : this(spriteRenderer) {
        Top = set.x;
        Bottom = set.y;
        Left = set.z;
        Right = set.w;
    }


}
