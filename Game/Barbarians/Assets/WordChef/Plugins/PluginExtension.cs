using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public static class PluginExtension
{
    public static void SetX(this Transform transform, float x)
    {
        Vector3 newPosition = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetY(this Transform transform, float y)
    {
        Vector3 newPosition = new Vector3(transform.position.x, y, transform.position.z);
        transform.position = newPosition;
    }

    public static void SetZ(this Transform transform, float z)
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, z);
        transform.position = newPosition;
    }

    public static void SetPosition2D(this Transform transform, Vector3 target)
    {
        Vector3 newPostion = new Vector3(target.x, target.y, transform.position.z);
        transform.position = newPostion;
    }

    public static void SetLocalX(this Transform transform, float x)
    {
        Vector3 newPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void SetLocalY(this Transform transform, float y)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void SetLocalZ(this Transform transform, float z)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
        transform.localPosition = newPosition;
    }

    public static void MoveLocalX(this Transform transform, float deltaX)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x + deltaX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void MoveLocalY(this Transform transform, float deltaY)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + deltaY, transform.localPosition.z);
        transform.localPosition = newPosition;
    }

    public static void MoveLocalZ(this Transform transform, float deltaZ)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + deltaZ);
        transform.localPosition = newPosition;
    }

    public static void MoveLocalXYZ(this Transform transform, float deltaX, float deltaY, float deltaZ)
    {
        Vector3 newPosition = new Vector3(transform.localPosition.x + deltaX, transform.localPosition.y + deltaY, transform.localPosition.z + deltaZ);
        transform.localPosition = newPosition;
    }

    public static void SetLocalScaleX(this Transform transform, float x)
    {
        Vector3 newScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
    }

    public static void SetLocalScaleY(this Transform transform, float y)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, y, transform.localScale.z);
        transform.localScale = newScale;
    }

    public static void SetLocalScaleZ(this Transform transform, float z)
    {
        Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y, z);
        transform.localScale = newScale;
    }

    public static void Times(this int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action();
        }
    }

    public static void SetColorAlpha(this MaskableGraphic graphic, float a)
    {
        Color newColor = graphic.color;
        newColor.a = a;
        graphic.color = newColor;
    }

    public static void LookAt2D(this Transform transform, Vector3 target, float angle = 0)
    {
        Vector3 dir = target - transform.position;
        angle += Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    public static void LookAt2D(this Transform transform, Transform target, float angle = 0)
    {
        LookAt2D(transform, target.position, angle);
    }

    public static float Delta(this float number, float delta)
    {
        return number + UnityEngine.Random.Range(-delta, delta);
    }

    public static float DeltaPercent(this float number, float percent)
    {
        return Delta(number, number * percent);
    }

    public static void Shuffle<T>(this IList<T> list, int? seed = null)
    {
        System.Random rng = seed != null ? new System.Random((int)seed) : new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
