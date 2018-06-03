using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UpDownNumber : MonoBehaviour {
    public bool hasLimit;
    public int max = 1000;
    public int min = 1;
    public int number = 1;
    public Action onNumberChanged;

    private void Start()
    {
        Number = number;
    }

    public int Number
    {
        get { return number; }
        set { 
            number = value;
            if (hasLimit)
            {
                gameObject.SetText(number + "/" + max);
            }
            else
            {
                gameObject.SetText(number.ToString());
            }
            if (onNumberChanged != null ) onNumberChanged();
        }
    }

    public void OnNumberChanged(int value) {
        Sound.instance.PlayButton();
        if (max == 0) return;
        if (hasLimit)
        {
            Number = Mathf.Clamp(number + value, min, max);
        }
        else
        {
            Number = Mathf.Max(number + value, min);
        }
    }
}
