using UnityEngine;
using System.Collections;

public class Guide : MonoBehaviour
{
    public enum Type { SwitchBalls, TipSkipBall, TipSwitchBalls };
    public Type type;
    public GameObject content;
    public float autoHideAfterTime = -1;

    public void Show()
    {
        content.SetActive(true);
        if (autoHideAfterTime != -1)
        {
            Invoke("Done", autoHideAfterTime);
        }
    }

    public void Done()
    {
        content.SetActive(false);
        SetDone(type, true);
    }

    public static void SetDone(Type type, bool isDone)
    {
        CPlayerPrefs.SetBool("is_guide_" + type.ToString() + "_done", isDone);
    }

    public static bool IsDone(Type type)
    {
        return CPlayerPrefs.GetBool("is_guide_" + type.ToString() + "_done");
    }
}
