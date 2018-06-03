using UnityEngine;
using System.Collections;

public class LimitRectSize : MonoBehaviour
{
    public bool landscape;
    [Range(0, 1)]
    public int matchWidthHeight;
    public float minWidth, minHeight, maxWidth, maxHeight;

    private float aspect, minAspect, maxAspect;
    private bool matchWidth;
    private float width, height;
    private RectTransform rectTransform;


    private void Start()
    {
        matchWidth = matchWidthHeight == 0;
        rectTransform = GetComponent<RectTransform>();

        if (landscape)
        {
            minAspect = minWidth / Mathf.Max(minHeight, maxHeight);
            maxAspect = maxWidth / Mathf.Min(minHeight, maxHeight);
        }
        else
        {
            minAspect = minHeight / Mathf.Max(minWidth, maxWidth);
            maxAspect = maxHeight / Mathf.Min(minWidth, maxWidth);
        }
    }

    private void Update()
    {
        if (landscape)
        {
            aspect = Screen.width / (float)Screen.height;
            float targetAspect = Mathf.Clamp(aspect, minAspect, maxAspect);
            if (matchWidth)
            {
                width = aspect <= maxAspect ? minWidth : minWidth * maxAspect / aspect;
                height = width / targetAspect;
            }
            else
            {
                height = aspect >= minAspect ? minHeight : minHeight * aspect / minAspect;
                width = height * targetAspect;
            }
        }
        else
        {
            aspect = Screen.height / (float)Screen.width;
            float targetAspect = Mathf.Clamp(aspect, minAspect, maxAspect);
            if (matchWidth)
            {
                width = aspect >= minAspect ? minWidth : minWidth * aspect / minAspect;
                height = width * targetAspect;
            }
            else
            {
                height = aspect <= maxAspect ? minHeight : minHeight * maxAspect / aspect;
                width = height / targetAspect;
            }
        }
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
