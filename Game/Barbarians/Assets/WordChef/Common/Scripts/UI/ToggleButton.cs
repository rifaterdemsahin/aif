using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ToggleButton : MonoBehaviour {

    public Button on, off;
    private bool isOn;

    public bool IsOn
    {
        get { return isOn; }
        set
        {
            isOn = value;
            UpdateButtons();
        }
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        on.gameObject.SetActive(isOn);
        off.gameObject.SetActive(!isOn);
    }
}
