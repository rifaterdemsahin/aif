using UnityEngine;
using System.Collections;

public class MarginObject : MonoBehaviour {

    public bool isMarginLeft, isMarginRight, isMarginTop, isMarginBottom;
    public float marginLeft, marginRight, marginTop, marginBottom;

    private void Start()
    {
        float width = UICamera.instance.GetWidth();
        float height = UICamera.instance.GetHeight();

        if (isMarginLeft)
        {
            transform.position = new Vector3(-width + marginLeft, transform.position.y, transform.position.z);
        }

        if (isMarginRight)
        {
            transform.position = new Vector3(width - marginRight, transform.position.y, transform.position.z);
        }

        if (isMarginTop)
        {
            transform.position = new Vector3(transform.position.x, height - marginTop, transform.position.z);
        }

        if (isMarginBottom)
        {
            transform.position = new Vector3(transform.position.x, -height + marginBottom, transform.position.z);
        }
    }
}
