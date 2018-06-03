using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RadioGroup : MonoBehaviour {

    public Image[] highlights;
    public string preferenceKey;
    private int selectedIndex;

    private void Start()
    {
        OnItemClick(CPlayerPrefs.GetInt(preferenceKey, 0));
    }

    public void OnItemClick(int index)
    {
        int count = 0;
        foreach (Image img in highlights)
        {
            if (count == index)
            {
                img.gameObject.SetActive(true);
            }
            else
            {
                img.gameObject.SetActive(false);
            }
            count++;
        }
        selectedIndex = index;
    }

    public void SaveChanged()
    {
        CPlayerPrefs.SetInt(preferenceKey, selectedIndex);
    }

    public int GetSelectedIndex()
    {
        return selectedIndex;
    }
}
