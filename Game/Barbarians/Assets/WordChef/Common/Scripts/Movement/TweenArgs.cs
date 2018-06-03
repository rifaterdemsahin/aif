using UnityEngine;
using System.Collections;

[System.Serializable]
public class TweenArgs {

    public float speed = 0.7f;
    public iTween.LoopType loopType = iTween.LoopType.none;
    public iTween.EaseType easeType = iTween.EaseType.linear;
}
