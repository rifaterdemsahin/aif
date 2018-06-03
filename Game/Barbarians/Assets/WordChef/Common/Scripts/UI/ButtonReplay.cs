using UnityEngine;
using System.Collections;
using System;

public class ButtonReplay : MyButton {
    public bool showConfirmDialog;
    public bool useScreenFader;

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        if (showConfirmDialog)
        {
            Action onYes = () =>
            {
                Replay();
            };
            DialogController.instance.ShowYesNoDialog(null, "Do you want to replay the game ?", onYes, null, DialogShow.STACK);
        }
        else
        {
            Replay();
        }
    }

    private void Replay()
    {
        CUtils.ReloadScene(useScreenFader);
    }
}
