using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RepeatingAction : MonoBehaviour {
    public string actionName = "";
    public int repeatRateSeconds = 1;
    public int inTimeSeconds = 0;

    public UnityEvent onActionReached;

    private void Start()
    {
        if (!CPlayerPrefs.HasKey(actionName + "_time")) // First time.
        {
            CUtils.SetActionTime(actionName, CUtils.GetCurrentTime() - repeatRateSeconds + inTimeSeconds);
        }

        UpdateAction();
    }

    public void UpdateAction()
    {
        if (CUtils.IsActionAvailable(actionName, repeatRateSeconds))
        {
            CUtils.SetActionTime(actionName);
            if (onActionReached != null) onActionReached.Invoke();
        }
    }
}
