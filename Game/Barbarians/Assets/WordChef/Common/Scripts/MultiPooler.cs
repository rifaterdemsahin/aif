using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiPooler : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private Stack<GameObject>[] arrayPooledObjects;

    private void Awake()
    {
        int len = objectPrefabs.Length;
        arrayPooledObjects = new Stack<GameObject>[len];
        for (int i = 0; i < len; i++)
        {
            arrayPooledObjects[i] = new Stack<GameObject>();
        }
    }

    public GameObject GetPooledObject(int index)
    {
        if (arrayPooledObjects[index].Count > 0)
        {
            GameObject obj = arrayPooledObjects[index].Pop();
            obj.SetActive(true);
            return obj;
        }
        return (GameObject)Instantiate(objectPrefabs[index]);
    }

    public void Push(GameObject obj, int index)
    {
        obj.SetActive(false);
        arrayPooledObjects[index].Push(obj);
    }

    public GameObject GetObjectPrefab(int index)
    {
        return objectPrefabs[index];
    }
}