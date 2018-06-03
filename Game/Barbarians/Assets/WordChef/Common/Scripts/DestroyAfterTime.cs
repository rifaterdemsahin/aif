using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DestroyAfterTime : MonoBehaviour {
    public float time;
    private void Start()
    {
        Destroy(gameObject, time);
    }
}
