using UnityEngine;
using System.Collections;

public class ExtraProgress : MonoBehaviour
{
    public float target;

    public float _current;
    private RectTransform rt;
    public float maxWidth;

    public float current
    {
        get { return _current; }
        set { _current = value; UpdateUI(); }
    }

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        maxWidth = rt.rect.width;
    }

    private void UpdateUI()
    {
        if (target == 0) return;

        float progress = Mathf.Clamp(current / target, 0, 1);
        rt.sizeDelta = new Vector2(maxWidth * progress, rt.rect.height);
    }
}
