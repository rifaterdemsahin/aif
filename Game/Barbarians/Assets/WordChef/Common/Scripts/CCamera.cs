using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class CCamera : MonoBehaviour
{
    public bool landscape;
    public float minWidth, minHeight, maxWidth, maxHeight;

    [HideInInspector]
    public float virtualWidth;
    [HideInInspector]
    public float virtualHeight;

    private int screenWidth, screenHeight;
    private float aspect;
    private Camera cam;
    private bool matchWidth;
    private float width;
    private float height;

    private float minAspect, maxAspect;

    public Action onScreenSizeChanged;

    protected virtual void Awake()
    {
        cam = GetComponent<Camera>();
        matchWidth = minWidth == maxWidth;

        if (landscape)
        {
            minAspect = minWidth / Mathf.Min(minHeight, maxHeight);
            maxAspect = maxWidth / Mathf.Max(minHeight, maxHeight);
        }
        else
        {
            minAspect = minHeight / Mathf.Max(minWidth, maxWidth);
            maxAspect = maxHeight / Mathf.Min(minWidth, maxWidth);
        }

        UpdateCamera();
    }

    private void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            UpdateCamera();
        }
    }

    private void UpdateCamera()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        if (landscape)
        {
            aspect = Screen.width / (float)Screen.height;
            SetUpForLandscape();
        }
        else
        {
            aspect = Screen.height / (float)Screen.width;
            SetUpForPortrait();
        }
        virtualHeight = cam.orthographicSize * 200;
        virtualWidth = landscape ? virtualHeight * aspect : virtualHeight / aspect;

        if (onScreenSizeChanged != null) onScreenSizeChanged();
    }

    private void SetUpForPortrait()
    {
        float targetAspect = Mathf.Clamp(aspect, minAspect, maxAspect);
        if (matchWidth)
        {
            width = minWidth / 200f;
            height = width * targetAspect;
        }
        else
        {
            height = minHeight / 200f;
            width = height / targetAspect;
        }
        cam.orthographicSize = aspect < maxAspect ? height : width * aspect;
    }

    private void SetUpForLandscape()
    {
        float targetAspect = Mathf.Clamp(aspect, minAspect, maxAspect);
        if (matchWidth)
        {
            width = minWidth / 200f;
            height = width / targetAspect;
        }
        else
        {
            height = minHeight / 200f;
            width = height * targetAspect;
        }
        cam.orthographicSize = aspect >= minAspect ? height : width / aspect;
    }

    // Half of the real height.
    public float GetHeight()
    {
        return height;
    }

    // Half of the real width.
    public float GetWidth()
    {
        return width;
    }
}
