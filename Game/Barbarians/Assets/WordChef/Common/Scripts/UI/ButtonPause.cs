using UnityEngine;
using System.Collections;

public class ButtonPause : MyButton {

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        DialogController.instance.ShowDialog(DialogType.Pause);
    }
}
