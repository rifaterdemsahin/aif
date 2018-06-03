using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InstantiateObject : MonoBehaviour {
    public GameObject gameObj;
    public bool isChild = true;
    public float delayTime = 0;

    [HideInInspector]
    public bool repeat = false;
    [HideInInspector]
    public bool infinity = true;
    [HideInInspector]
    public int numRepeat;
    [HideInInspector]
    public float gapTime;

    private void Start()
    {
        if (repeat && gapTime == 0)
        {
            Debug.LogError("You must set gap time between each repeat.");
            return;
        }

        StartCoroutine(DoWork());
    }

    private IEnumerator DoWork()
    {
        yield return new WaitForSeconds(delayTime);
        int count = 0;
        while (true)
        {
            GameObject clone = (GameObject)Instantiate(gameObj, transform.position, transform.rotation);
            clone.transform.SetParent(isChild ? transform : null);
            clone.transform.localScale = Vector3.one;
            count ++;
            if (repeat == false) break;

            yield return new WaitForSeconds(gapTime);
            if (infinity == false && count == numRepeat + 1)
            {
                break;
            }
        }
    }
}
