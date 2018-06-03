using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour {

    private const int SHARE_RUBY = 5, INVITE_RUBY = 5;
    private const int SHARE_PERIOD = 24 * 3600;

    private string pendingAction = null;

	private void Start ()
    {
        FacebookController.instance.onFacebookLoginComplete += OnFacebookLoginComplete;
        FacebookController.instance.onShareLinkComplete += OnShareGameComplete;
        FacebookController.instance.onAppRequestComplete += OnAppRequestComplete;
        FacebookController.instance.onGetInvitableFriendsComplete += OnGetInvitableFriendsComplete;
    }

    public void OnShareGameClick()
    {
        float remain = (float)(SHARE_PERIOD - CUtils.GetActionDeltaTime("share_facebook"));
        if (remain > 0 && remain < SHARE_PERIOD)
        {
            string message;
            if (remain > 3600)
            {
                float value = Mathf.Round(remain / 3600);
                message = value == 1 ? "an hour" : value + " hours";
            }
            else if (remain > 60)
            {
                float value = Mathf.Round(remain / 60);
                message = value == 1 ? "a minute" : value + " minutes";
            }
            else
            {
                float value = Mathf.Round(remain);
                message = value == 1 ? "a second" : value + " seconds";
            }

            Toast.instance.ShowMessage("Please wait " + message + " for the next share");
            return;
        }

        if (FB.IsLoggedIn)
        {
            FacebookController.instance.ShareLink();
        }
        else
        {
            FacebookController.instance.LoginFacebook();
            pendingAction = "share";
        }
    }

    public void OnInviteFriendClick()
    {
        if (FB.IsLoggedIn)
        {
            bool ok = FacebookController.instance.CustomInviteFriends();
            if (!ok) pendingAction = "invite";
        }
        else
        {
            FacebookController.instance.LoginFacebook();
            pendingAction = "invite";
        }
    }

    private void OnFacebookLoginComplete()
    {
        if (pendingAction == "share")
        {
            FacebookController.instance.ShareLink();
            pendingAction = null;
        }
        else if (pendingAction == "invite")
        {
            bool ok = FacebookController.instance.CustomInviteFriends();
            if (ok) pendingAction = null;
        }
    }

    private void OnGetInvitableFriendsComplete(List<InvitableFriend> invitableFriends)
    {
        if (pendingAction == "invite")
        {
            FacebookController.instance.CustomInviteFriends();
            pendingAction = null;
        }
    }

    private void OnShareGameComplete()
    {
        Toast.instance.ShowMessage("You've received " + SHARE_RUBY + " rubies");
        CurrencyController.CreditBalance(SHARE_RUBY);
        CUtils.SetActionTime("share_facebook");
    }

    private void OnAppRequestComplete()
    {
        Toast.instance.ShowMessage("You've received " + INVITE_RUBY + " rubies");
        CurrencyController.CreditBalance(INVITE_RUBY);
    }

    private void OnDestroy()
    {
        FacebookController.instance.onFacebookLoginComplete -= OnFacebookLoginComplete;
        FacebookController.instance.onShareLinkComplete -= OnShareGameComplete;
        FacebookController.instance.onAppRequestComplete -= OnAppRequestComplete;
        FacebookController.instance.onGetInvitableFriendsComplete -= OnGetInvitableFriendsComplete;
    }
}
