using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TButton : MyButton {

    public bool isOn;
    public Sprite on, off;

    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            UpdateButtons();
        }
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        IsOn = !IsOn;
    }

    public void UpdateButtons()
    {
        GetComponent<Image>().sprite = isOn ? on : off;
    }
}
