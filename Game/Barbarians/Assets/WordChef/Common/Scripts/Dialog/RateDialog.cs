using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateDialog : YesNoDialog {

	public override void OnYesClick()
    {
        base.OnYesClick();
        CUtils.RateGame();
    }

    public override void OnNoClick()
    {
        base.OnNoClick();
    }
}
