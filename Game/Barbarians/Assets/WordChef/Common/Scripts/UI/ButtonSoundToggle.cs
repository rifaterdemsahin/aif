using UnityEngine;
using System.Collections;

public class ButtonSoundToggle : TButton {

    protected override void Start()
    {
        base.Start();
        IsOn = Sound.instance.IsEnabled();
    }

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        Sound.instance.SetEnabled(IsOn);
    }
}
