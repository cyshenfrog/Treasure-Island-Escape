using UnityEngine;
using System.Collections;

public abstract class Form
{
    public abstract bool Inside(Vector2 position);

    public Vector2 Center
    {
        get { return center; }
    }

    protected Vector3 rotation;
    protected Vector2 center;
    protected MapConstants.FormType formType;
}
