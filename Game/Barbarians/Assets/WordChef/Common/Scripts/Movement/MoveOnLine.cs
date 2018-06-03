using UnityEngine;
using System.Collections;

public class MoveOnLine : MonoBehaviour
{
    public Transform pointA, pointB;
    public float speed = 0.7f;
    public iTween.EaseType easetype = iTween.EaseType.linear;

    protected virtual void Start()
    {
        if (!CUtils.EqualVector3(transform.position, pointA.position))
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", pointA.position, "speed", speed, "easeType", easetype, "oncomplete", "OnMoveToPointComplete"));
        }
        else
        {
            OnMoveToPointComplete();
        }
    }

    private void OnMoveToPointComplete()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", pointB.position, "looptype", "pingpong", "speed", speed, "easeType", easetype));
    }
}
