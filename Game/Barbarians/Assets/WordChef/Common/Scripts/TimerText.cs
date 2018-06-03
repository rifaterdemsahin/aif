using UnityEngine;
using System.Collections;
using System;

public class TimerText : MonoBehaviour
{
    public bool countUp = true;
    public bool runOnStart = false;
    public int timeValue = 0;

    public bool showHour = false;
    public bool showMinute = true;
    public bool showSecond = true;

    public Action onCountDownComplete;

    private bool isRunning = false;

    private void Start()
    {
        UpdateText();
        if (runOnStart)
        {
            Run();
        }
    }

    public void Run()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(UpdateClockText());
        }
    }

    private IEnumerator UpdateClockText()
    {
        while (isRunning)
        {
            UpdateText();
            yield return new WaitForSeconds(1);
            if (countUp) timeValue++;
            else
            {
                if (timeValue == 0)
                {
                    if (onCountDownComplete != null) onCountDownComplete();
                    Stop();
                }
                else timeValue--;
            }
        }
    }

    public void SetTime(int value)
    {
        if (value < 0)
            value = 0;
        timeValue = value;
        UpdateText();
    }

    public void AddTime(int value)
    {
        timeValue += value;
        UpdateText();
    }

    private void UpdateText()
    {
        TimeSpan t = TimeSpan.FromSeconds(timeValue);

        string text;
        if (showHour && showMinute && showSecond)
        {
            text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
        }
        else if (showHour && showMinute)
        {
            text = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
        }
        else
        {
            text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
        }
        gameObject.SetText(text);
    }

    public void Stop()
    {
        isRunning = false;
    }
}
