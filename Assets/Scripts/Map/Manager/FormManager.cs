using UnityEngine;
using System.Collections;

public static class FormManager
{
    public static Form Factory(MapConstants.FormType formType, int width, int height)
    {
        //to calculate arguments
        switch(formType)
        {
            case MapConstants.FormType.Ellipse:
                return new Ellipse(.5f * (width * Vector2.right + height * Vector2.up), width * .4f, height * .4f);
            default:
                Debug.LogError("FormManager Factory Error: unexcepted FormType");
                return null;
        }
    }
}
