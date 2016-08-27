using UnityEngine;
using System.Collections;

public class TargetBar : MonoBehaviour {
    public VerticalLayoutGroupResizer ScrollContent;

    /// <summary>
    /// Add an element to this bar and resize the layout
    /// </summary>
    /// <param name="element">The transform of the element to be added</param>
    /// <param name="doesResize">Does the layout resize</param>
    public void AddElement(Transform element, bool doesResize)
    {
        Vector3 position = element.transform.position;
        position.z = transform.position.z;
        element.transform.position = position;
        element.SetParent(ScrollContent.transform);
        element.localScale = Vector3.one;

        if (doesResize)
            ScrollContent.Resize();
    }
    /// <summary>
    /// Resize the layout
    /// </summary>
    public void Resize()
    {
        ScrollContent.Resize();
    }
}
