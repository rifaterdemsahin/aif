using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public class ScaleItem
{
    public bool enabled = false;
    public float scaleNormal = 1;
    public float scaleTo = 1.2f;
    public float time = 0.3f;
}

public class SnapScrollRect : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [HideInInspector]
    public int screens;
    float[] points;
    public int speed = 10;
    float stepSize;

    ScrollRect scroll;
    bool lerp;
    float target;
    [HideInInspector]
    public int index = 0;

    public Transform indicatorRoot;
    [HideInInspector]
    public GameObject[] indicators;
    public Text tabName;
    public string tabNamePrefix;

    public ScaleItem scaleItem;

    public Action<int> onPageChanged;

    void Awake()
    {
        scroll = gameObject.GetComponent<ScrollRect>();

        screens = scroll.content.childCount;
        if (screens != 0) InitPoints(screens);

        indicators = new GameObject[indicatorRoot.childCount];
        int i = 0;
        foreach(Transform child in indicatorRoot)
        {
            indicators[i] = child.GetChild(1).gameObject;
            i++;
        }
    }

    public void InitPoints(int _screens)
    {
        screens = _screens;
        points = new float[screens];

        if (screens > 1)
        {
            stepSize = 1 / (float)(screens - 1);

            for (int i = 0; i < screens; i++)
            {
                points[i] = i * stepSize;
            }
        }
        else
        {
            points[0] = 0;
        }
    }

    void Update()
    {
        if (lerp == false) return;
        if (scroll.horizontal)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, target, speed * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.horizontalNormalizedPosition, target)) lerp = false;
        }
        else
        {
            scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, target, speed * scroll.elasticity * Time.deltaTime);
            if (Mathf.Approximately(scroll.verticalNormalizedPosition, target)) lerp = false;
        }
    }

    private float beginDragTimer;
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDragTimer = Time.time;
    }

    private int direction;
    public void OnEndDrag(PointerEventData data)
    {
        float dragTime = Time.time - beginDragTimer;
        if (dragTime > 0.15f)
        {
            index = scroll.horizontal ? FindNearest_SlowSwipe(scroll.horizontalNormalizedPosition, points) :
                         scroll.vertical ? FindNearest_SlowSwipe(scroll.verticalNormalizedPosition, points) : index;
            MoveToPage(index);
        }
        else
        {
            direction = scroll.velocity.x > 0 ? -1 : 1;
            Timer.Schedule(this, 0.1f, () =>
            {
                index = scroll.horizontal ? FindNearest_FastSwipe(scroll.horizontalNormalizedPosition, points) :
                             scroll.vertical ? FindNearest_FastSwipe(scroll.verticalNormalizedPosition, points) : index;
                MoveToPage(index);
            });
        }
    }

    public void NextPage()
    {
        MoveToPage(index + 1);
        Sound.instance.PlayButton();
    }

    public void PreviousPage()
    {
        MoveToPage(index - 1);
        Sound.instance.PlayButton();
    }

    public void MoveToPage(int pageIndex)
    {
        index = Mathf.Clamp(pageIndex, 0, screens - 1);
        target = points[index];
        lerp = true;
        UpdateIndicator();

        if (onPageChanged != null) onPageChanged(index);
        if (scaleItem.enabled) ScaleEffect(index);
    }

    public void SetPage(int pageIndex)
    {
        index = Mathf.Clamp(pageIndex, 0, screens - 1);
        target = points[index];
        if (scroll.horizontal) scroll.horizontalNormalizedPosition = target;
        else scroll.verticalNormalizedPosition = target;

        UpdateIndicator();
        if (onPageChanged != null) onPageChanged(index);
        if (scaleItem.enabled) ScaleEffect(index, true);
    }

    public void UpdateIndicator()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].SetActive(i == index);
        }
        if (tabName != null)
        {
            tabName.text = tabNamePrefix + (index + 1);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        lerp = false;
    }

    int FindNearest_SlowSwipe(float f, float[] array)
    {
        float distance = Mathf.Infinity;
        int output = 0;
        for (int index = 0; index < array.Length; index++)
        {
            if (Mathf.Abs(array[index] - f) < distance)
            {
                distance = Mathf.Abs(array[index] - f);
                output = index;
            }
        }
        return output;
    }

    int FindNearest_FastSwipe(float f, float[] array)
    {
        float sigment = array[1] - array[0];
        int output = direction == 1 ? array.Length - 1 : 0;

        for (int i = index; i >= 0 && i < array.Length; i += direction)
        {
            if (Mathf.Abs(i - index) <= 1 && Mathf.Abs(array[i] - f) < 0.2f * sigment)
            {
                output = i;
                break;

            }
            else if (direction == 1 && array[i] - f > 0 || direction == -1 && array[i] - f < 0)
            {
                output = i;
                break;
            }
        }

        return output;
    }

    private void ScaleEffect(int index, bool immediate = false)
    {
        int i = 0;
        foreach (Transform item in scroll.content.transform)
        {
            if (immediate)
            {
                item.localScale = Vector3.one * (index == i ? scaleItem.scaleTo : scaleItem.scaleNormal);
            }
            else
            {
                iTween.ScaleTo(item.gameObject, Vector3.one * (index == i ? scaleItem.scaleTo : scaleItem.scaleNormal), scaleItem.time);
            }
            i++;
        }
    }
}