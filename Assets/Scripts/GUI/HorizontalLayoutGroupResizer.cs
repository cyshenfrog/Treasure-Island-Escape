using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HorizontalLayoutGroupResizer : MonoBehaviour {
    public float ElementWidth;
    public void Resize()
    {
        int childCount = transform.childCount;
        HorizontalLayoutGroup layout = GetComponent<HorizontalLayoutGroup>();
        RectOffset padding = layout.padding;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            (childCount - 1) * layout.spacing + padding.horizontal + ElementWidth * childCount);
    }
}
