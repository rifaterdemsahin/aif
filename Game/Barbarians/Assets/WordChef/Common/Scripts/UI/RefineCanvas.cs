using UnityEngine;
using UnityEngine.UI;

public class RefineCanvas : MonoBehaviour
{
    public RectTransform[] canvases;
    public RectTransform[] screensizeCanvases;

    private void Start()
    {
        OnScreenSizeChanged();
        UICamera.instance.onScreenSizeChanged += OnScreenSizeChanged;
    }

    private void OnScreenSizeChanged()
    {
        foreach (RectTransform rTransform in canvases)
        {
            rTransform.sizeDelta = new Vector2(UICamera.instance.GetWidth() * 200, UICamera.instance.GetHeight() * 200);
        }
        foreach (RectTransform rTransform in screensizeCanvases)
        {
            rTransform.sizeDelta = new Vector2(UICamera.instance.virtualWidth, UICamera.instance.virtualHeight);
        }
    }
}