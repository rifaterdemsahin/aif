using UnityEngine;
using System.Collections;

public class ShowPromoteDialog : MonoBehaviour {

	public void Show()
    {
        Timer.Schedule(this, 0.6f, () =>
        {
            var promote = PromoteController.instance.GetPromote(PromoteType.PopupDialog);
            if (promote != null)
            {
                DialogController.instance.ShowDialog(DialogType.PromotePopup, DialogShow.DONT_SHOW_IF_OTHERS_SHOWING);
            }
        });
    }
}
