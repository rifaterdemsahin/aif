using UnityEngine;
using System.Collections;

public class MoveOnCircle : MonoBehaviour {
    public float radius = 1;
    public float speed = 1f;
    public float currentAngle = 0;

    public Transform center;

	private void Update()
    {
        currentAngle += Time.deltaTime * speed;
        transform.position = new Vector3(center.position.x + Mathf.Cos(currentAngle) * radius, center.position.y + Mathf.Sin(currentAngle) * radius);
    }
}
