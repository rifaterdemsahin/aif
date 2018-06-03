using UnityEngine;
using System.Collections;

public class GuideController : MonoBehaviour
{
    private Guide[] guides;
    public static GuideController instance;

    private void Awake()
    {
        instance = this;
    }

    private void UpdateList()
    {
        guides = FindObjectsOfType<Guide>();
    }

    public void Show(Guide.Type type)
    {
        UpdateList();
        foreach (var guide in guides)
        {
            if (type == guide.type)
            {
                guide.Show();
            }
        }
    }

    public bool IsShowing(Guide.Type type)
    {
        if (guides == null) return false;
        foreach (var guide in guides)
        {
            if (type == guide.type)
            {
                return guide.content.activeSelf;
            }
        }
        return false;
    }

    public void Done(Guide.Type type)
    {
        UpdateList();
        foreach (var guide in guides)
        {
            if (type == guide.type)
            {
                guide.Done();
            }
        }
    }
}
