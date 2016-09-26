using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StateBar : MonoBehaviour {
    public AnimationCurve curve;
    public float time;
    public float maxWidth, minWidth;
    public RectTransform target;
    private float currentValue;
    private float maxValue = 100;
    private float originalValue, targetValue;
    private float currentTime;
    public float theValue { get; protected set; }
    public float MaxValue
    {
        get
        {
            return maxValue;
        }
        set
        {
            if (enabled)
            {
                Debug.LogWarning("It seems that here is setting value. This change for max value will be ignored.");
                return;
            }
            maxValue = value;
        }
    }
    public void SetValueRapidly(float value)
    {
        enabled = false;
        theValue = currentValue = Mathf.Clamp(value, maxValue, 0);
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(0, maxValue, theValue / maxValue));
    }
    public void SetValue(float value)
    {
        originalValue = currentValue;
        theValue = targetValue = Mathf.Clamp(value, 0, maxValue);
        currentTime = 0;
        enabled = true;
    }
    void Awake()
    {
        if (enabled)
        {
            Debug.LogWarning("Please disable this component before play, or don't set value (not rapidly) on awake.");
        }
        currentValue = theValue = maxValue;
        enabled = false;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= time)
        {
            currentTime = time;
            enabled = false;
        }
        currentValue = Mathf.Lerp(originalValue, targetValue, curve.Evaluate(currentTime / time));
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            Mathf.Lerp(minWidth, maxWidth, currentValue / maxValue));
    }
}
