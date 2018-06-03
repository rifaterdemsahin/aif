using UnityEngine;

public class TabBehaviour : MonoBehaviour
{
    public GameObject[] tabButtons;
    public GameObject[] tabContents;
    public GameObject[] tabNameObjects;
    public GameObject[] selectedTabs;

    public string[] tabName;

    protected int currentTab;

    private void Start()
    {
        for(int i = 0; i < tabContents.Length; i++)
        {
            if (tabContents[i].activeSelf)
            {
                currentTab = i;
                break;
            }
        }
    }

    public void TabChanged(int index)
    {
        Sound.instance.PlayButton();
        currentTab = index;
        for (int i = 0; i < tabButtons.Length; i++)
        {
            TabVisible(i);
            if (i == index)
            {
                TabSelected(i);
            }
            else
            {
                TabUnselected(i);
            }
        }
    }

    protected virtual void TabVisible(int index)
    {

    }

    protected virtual void TabSelected(int index)
    {
        tabContents[index].SetActive(true);
        selectedTabs[index].SetActive(true);
    }

    protected virtual void TabUnselected(int index)
    {
        tabContents[index].SetActive(false);
        selectedTabs[index].SetActive(false);
    }

    public void SetCurrentTab(int index)
    {
        TabChanged(index);
    }
}
