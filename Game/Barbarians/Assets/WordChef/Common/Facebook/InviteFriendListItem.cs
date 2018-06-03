using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class InviteFriendListItem : MonoBehaviour
{
    public Text nameText;
    public Photo photo;
    public Toggle toggleCheck;

    public Action onToggleChanged; 

    public void Build(string name, string avatarUrl)
    {
        if (name.Length > 6) name = name.Substring(0, 6);
        nameText.text = name;
        photo.url = avatarUrl;
        photo.SetSize(90, 90);
        photo.SetRealSize(99, 99);
        photo.Load();
    }

    public void OnToggleClick()
    {
        Sound.instance.PlayButton();
        toggleCheck.isOn = !toggleCheck.isOn;
        if (onToggleChanged != null) onToggleChanged();
    }
}
