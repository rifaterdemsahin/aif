using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
    public GameObject objectPrefab;
    private Stack<GameObject> pooledObjects;

    private void Awake()
    {
        pooledObjects = new Stack<GameObject>();
    }

    public GameObject GetPooledObject()
    {
        if (pooledObjects.Count > 0)
        {
            GameObject obj = pooledObjects.Pop();
            obj.SetActive(true);
            return obj;
        }
        return (GameObject)Instantiate(objectPrefab);
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        pooledObjects.Push(obj);
    }
}