using UnityEngine;
using System.Collections;

public class ButtonMusicToggle : TButton {

    protected override void Start()
    {
        base.Start();
        IsOn = Music.instance.IsEnabled();
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        Music.instance.SetEnabled(IsOn, true);
    }
}
