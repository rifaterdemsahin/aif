using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;

public class InviteFriendsDialog : Dialog
{
    public InviteFriendListItem inviteFriendListItem;
    public RectTransform contentTransform;
    public Toggle selectAllToggle;
    public Text numFriendSelected;

    private List<InvitableFriend> data;

    public void Build(List<InvitableFriend> data)
    {
        this.data = data;

        for (int i = 0; i < data.Count; i++)
        {
            string name = data[i].name;
            string avatarUrl = data[i].avatarUrl;
            InviteFriendListItem item = Instantiate(inviteFriendListItem);
            item.Build(name, avatarUrl);
            item.transform.SetParent(contentTransform);
            item.GetComponent<Transform>().localScale = Vector3.one;
            item.onToggleChanged += UpdateSelectAllToggle;
        }
        contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, 110 * data.Count);
    }

    protected override void Start()
    {
        base.Start();
        UpdateNumFriendSelected();
    }

    public override void Show()
    {
        base.Show();
        CUtils.CloseBannerAd();
    }

    public override void Close()
    {
        base.Close();
        CUtils.ShowBannerAd();
    }

    public void Invite()
    {
        Sound.instance.PlayButton();
        if (CheckRequirement())
        {
            string recipients = GetRecipients();
            FacebookController.instance.InviteFriends(recipients);
            Close();
        }
    }

    public void SelectAllToggleClick()
    {
        for (int i = 0; i < data.Count; i++)
        {
            InviteFriendListItem item = contentTransform.GetChild(i).GetComponent<InviteFriendListItem>();
            item.toggleCheck.isOn = selectAllToggle.isOn;
        }

        UpdateNumFriendSelected();
    }

    private void UpdateSelectAllToggle()
    {
        bool isOn = true;
        for (int i = 0; i < data.Count; i++)
        {
            InviteFriendListItem item = contentTransform.GetChild(i).GetComponent<InviteFriendListItem>();
            if (item.toggleCheck.isOn == false)
            {
                isOn = false;
                break;
            }
        }

        selectAllToggle.isOn = isOn;
        UpdateNumFriendSelected();
    }

    private void UpdateNumFriendSelected()
    {
        int total = 0;
        for (int i = 0; i < data.Count; i++)
        {
            InviteFriendListItem item = contentTransform.GetChild(i).GetComponent<InviteFriendListItem>();
            if (item.toggleCheck.isOn)
                total++;
        }

        numFriendSelected.text = total == 0 ? "No friend is selected" :
                                 total == 1 ? "1 friend is selected" :
                                 total + " friends are selected";
    }

    private bool CheckRequirement()
    {
        int numofChecked = 0;
        for (int i = 0; i < data.Count; i++)
        {
            InviteFriendListItem item = contentTransform.GetChild(i).GetComponent<InviteFriendListItem>();
            data[i].shouldInvite = item.toggleCheck.isOn;
            if (item.toggleCheck.isOn)
                numofChecked++;
        }

        if (numofChecked >= CommonConst.MIN_INVITE_FRIEND)
        {
            return true;
        }
        Toast.instance.ShowMessage("You need to invite at least 40 friends");
        return false;
    }

    private string GetRecipients()
    {
        List<string> ids = new List<string>();
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].shouldInvite == false) continue;
            ids.Add(data[i].id);
        }

        return string.Join(",", ids.ToArray());
    }
}
