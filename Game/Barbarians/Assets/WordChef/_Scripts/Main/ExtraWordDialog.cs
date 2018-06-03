using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraWordDialog : Dialog
{
    public Transform claimTr;
    public ExtraProgress extraProgress;
    public GameObject claimButton;
    public Text progressText;

    private int numWords, claimQuantity;

    protected override void Start()
    {
        base.Start();
        extraProgress.target = Prefs.extraTarget;
        extraProgress.current = Prefs.extraProgress;

        UpdateUI();
    }

    public void Claim()
    {
        claimQuantity = (int)extraProgress.target / 5 * 2;
        
        extraProgress.current -= (int)extraProgress.target;
        Prefs.extraProgress = (int)extraProgress.current;
        UpdateUI();

        StartCoroutine(ClaimEffect());
        ExtraWord.instance.OnClaimed();

        if (Prefs.extraTarget == 5 && Prefs.totalExtraAdded > 10)
        {
            Prefs.extraTarget = 10;
            extraProgress.target = 10;
            UpdateUI();
        }
    }

    private IEnumerator ClaimEffect()
    {
        Transform rubyBalance = GameObject.FindWithTag("RubyBalance").transform;
        var middlePoint = CUtils.GetMiddlePoint(claimTr.position, rubyBalance.position, -0.4f);
        Vector3[] waypoints = { claimTr.position, middlePoint, rubyBalance.position };

        for (int i = 0; i < claimQuantity; i++)
        {
            GameObject gameObj = Instantiate(MonoUtils.instance.rubyFly);
            gameObj.transform.position = waypoints[0];
            gameObj.transform.localScale = 0.5f * Vector3.one;

            iTween.MoveTo(gameObj, iTween.Hash("path", waypoints, "speed", 30, "oncomplete", "OnMoveComplete"));
            iTween.ScaleTo(gameObj, iTween.Hash("scale", 0.7f * Vector3.one, "time", 0.3f));
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateUI()
    {
        claimButton.SetActive(extraProgress.current >= extraProgress.target);
        progressText.text = extraProgress.current + "/" + extraProgress.target;
    }
}
