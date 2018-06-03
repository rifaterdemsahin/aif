using UnityEngine;
using System.Collections;

public class CRotate : MonoBehaviour {
    public float speed;

	private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
