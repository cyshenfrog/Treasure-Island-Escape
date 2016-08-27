using UnityEngine;
using UnityEngine.UI;

public class VerticalLayoutGroupResizer : MonoBehaviour {
    public float ElementHeight;

    public void Resize()
    {
        int childCount = transform.childCount;
        VerticalLayoutGroup layout = GetComponent<VerticalLayoutGroup>();
        RectOffset padding = layout.padding;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            (childCount - 1) * layout.spacing + padding.vertical + ElementHeight * childCount);
    }
}
