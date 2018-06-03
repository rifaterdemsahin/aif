using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CButton : MonoBehaviour {
    public Image[] images;
    public Sprite[] activeSprites;
    public Sprite[] inactiveSprites;
    public Color activeTextColor, inActiveTextColor;
    public Button button;
    public GameObject buttonText;

    private bool isActive = true;

    public bool IsActive()
    {
        return isActive;
    }

    public void SetText(string text)
    {
        if (buttonText != null)
        {
            buttonText.SetText(text);
        }
    }

    public void SetActiveSprite(bool isActive)
    {
        this.isActive = isActive;
        if (button != null)
        {
            button.interactable = isActive;
        }

        if (buttonText != null && !activeTextColor.Equals(Color.clear) && !inActiveTextColor.Equals(Color.clear))
        {
            buttonText.GetComponent<Text>().color = isActive ? activeTextColor : inActiveTextColor;
        }

        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = isActive ? activeSprites[i] : inactiveSprites[i];
        }
    }
}
