using UnityEngine;
using System.Collections;
using System;

public class ClockText : MonoBehaviour
{
    public string action;
    public int duration;
    public string contentWhenComplete = "";
    public bool availableFirstTime = true;

    public Action onClockComplete;
    private int remainingTime;

    public void ShowClockText()
    {
        if (!CUtils.IsActionAvailable(action, duration, availableFirstTime))
        {
            int delta = (int)(CUtils.GetCurrentTime() - CUtils.GetActionTime(action));
            remainingTime = duration - delta;
            StartCoroutine(UpdateClockText());
        }
        else
        {
            ClockComplete();
        }
    }

    public void UpdateTimeClockText()
    {
        if (!CUtils.IsActionAvailable(action, duration, availableFirstTime))
        {
            int delta = (int)(CUtils.GetCurrentTime() - CUtils.GetActionTime(action));
            remainingTime = duration - delta;
            UpdateText();
        }
    }

    private IEnumerator UpdateClockText()
    {
        while (remainingTime > 0)
        {
            UpdateText();
            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        ClockComplete();
    }

    private void UpdateText()
    {
        TimeSpan t = TimeSpan.FromSeconds(remainingTime);
        string text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        gameObject.SetText(text);
    }

    private void ClockComplete()
    {
        gameObject.SetText(contentWhenComplete);
        if (onClockComplete != null) onClockComplete();
    }
}
