using UnityEngine;
using System.Collections;

public class ButtonFacebookPage : MyButton {
    public string facebookPageID;

    public override void OnButtonClick()
    {
        base.OnButtonClick();
        CUtils.LikeFacebookPage(facebookPageID);
    }
}
