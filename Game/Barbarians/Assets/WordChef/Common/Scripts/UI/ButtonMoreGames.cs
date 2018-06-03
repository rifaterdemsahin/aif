using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonMoreGames : MyButton {

    public override void OnButtonClick()
    {
        base.OnButtonClick();
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Superpow");
#elif UNITY_IPHONE
        Application.OpenURL("https://itunes.apple.com/us/developer/dong-tran/id961632428");
#endif
    }
}
