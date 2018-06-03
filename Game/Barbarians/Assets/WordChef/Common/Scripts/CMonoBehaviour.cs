using UnityEngine;
using System.Collections.Generic;

public class CMonoBehaviour : MonoBehaviour
{
    public void DestroyIfExist(Transform objTransform)
    {
        if (objTransform != null)
        {
            Destroy(objTransform.gameObject);
        }
    }
}
