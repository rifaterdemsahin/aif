using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnAndMoveObjects : MonoBehaviour
{
    public GameObject prefab;
    public int quality;
    public float gapTime;
    public TweenArgs tweenArgs;

    public Transform[] path;
    private Vector3[] waypoints;

    public IEnumerator DoTask()
    {
        if (waypoints == null) waypoints = path.Select(x => x.position).ToArray();
        for (int i = 0; i < quality; i++)
        {
            GameObject gameObj = Instantiate(prefab);
            gameObj.transform.position = waypoints[0];
            iTween.MoveTo(gameObj, iTween.Hash("path", waypoints, "speed", tweenArgs.speed, "easeType", tweenArgs.easeType, "oncomplete", "OnMoveComplete"));
            yield return new WaitForSeconds(gapTime);
        }
    }

    public void SetWaypoints(params Vector3[] waypoints)
    {
        this.waypoints = waypoints;
    }
}
