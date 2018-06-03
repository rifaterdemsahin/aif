using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ScrollViewContent : MonoBehaviour, IPointerClickHandler
{
    public SnapScrollRect snapScroll;
    public int numItems = 1;

    public void OnPointerClick(PointerEventData ped)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), ped.position, ped.pressEventCamera, out localCursor))
            return;

        int index = (int)(localCursor.x / (GetComponent<RectTransform>().sizeDelta.x / numItems));
        snapScroll.MoveToPage(index);
    }
}
