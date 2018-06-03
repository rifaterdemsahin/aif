using UnityEngine;
using System.Collections;
using System.Linq;

public class MoveOnPath : MonoBehaviour {

    public Transform[] paths;
    public float speed = 0.7f;
    public iTween.LoopType loopType = iTween.LoopType.none;
    public iTween.EaseType easeType = iTween.EaseType.linear;

    public int currentIndex = 0;

    private int plus = 1;
    private Vector3[] waypoints;

    private void Start()
    {
        if (waypoints == null) waypoints = paths.Select(x => x.position).ToArray();
        MoveToPointComplete();
    }

    public void SetWaypoints(Vector3[] waypoints)
    {
        this.waypoints = waypoints;
    }

    private void MoveToPointComplete()
    {
        if (loopType == iTween.LoopType.none)
        {
            if (currentIndex == waypoints.Length - 1) return;
            currentIndex++;
        }
        else if (loopType == iTween.LoopType.loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else if (loopType == iTween.LoopType.pingPong)
        {
            plus = (plus == -1 && currentIndex == 0) || currentIndex == waypoints.Length - 1 ? -plus : plus;
            currentIndex += plus;
        }

        Vector3 target = waypoints[currentIndex];
        iTween.MoveTo(gameObject, iTween.Hash("position", target, "speed", speed, "easeType", easeType, "oncomplete", "MoveToPointComplete"));
    }
}
