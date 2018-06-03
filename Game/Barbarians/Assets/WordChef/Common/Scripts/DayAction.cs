using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class DayAction : MonoBehaviour {
    public int dayInterval = 1;
    public string actionName = "";
    public UnityEvent onActionReached;

    private void Start()
    {
        int currentDay = (int)CUtils.GetCurrentTimeInDays();
        int day = CPlayerPrefs.GetInt("day_action_" + actionName, -1);

        if (currentDay - day >= dayInterval)
        {
            CPlayerPrefs.SetInt("day_action_" + actionName, currentDay);
            if (onActionReached != null) onActionReached.Invoke();
        } 
    }
}
