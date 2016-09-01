using UnityEngine;
using System.Collections;

public static class FormManager
{
    public static Form Factory(MapConstants.FormType formType, int width, int height)
    {
        //to calculate arguments
        //..

        switch(formType)
        {
            case MapConstants.FormType.Ellipse:
                return new Ellipse(Vector2.zero, 1f, 1f);
            default:
                Debug.LogError("FormManager Factory Error: unexcepted FormType");
                return null;
        }
    }
}
