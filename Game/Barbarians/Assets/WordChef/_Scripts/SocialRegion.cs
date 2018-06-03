using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class SocialRegion : MonoBehaviour {
    public GameObject overlay, panel, bottomBanner;
    public Transform panelIn, panelOut;
    public bool isShowing;

    public static SocialRegion instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowPanel()
    {
        overlay.SetActive(true);
        panel.SetActive(true);
        panel.transform.position = panelOut.position;
        iTween.MoveTo(panel, panelIn.position, 0.3f);
        isShowing = true;
    }

    public void HidePanel()
    {
        overlay.SetActive(false);
        iTween.MoveTo(panel, panelOut.position, 0.3f);
        isShowing = false;
    }

    public void OnSocialClick()
    {
        if (ConfigController.Config.enableFacebookFeatures)
            ShowPanel();
        else
            OnAskFriendClick();
    }

    public void OnAskFriendClick()
    {
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
        StartCoroutine(CROneStepSharing());
#else
        Toast.instance.ShowMessage("This function only works on Android and iOS");
#endif
    }

    IEnumerator CROneStepSharing()
    {
        yield return new WaitForEndOfFrame();
        HidePanel();
        yield return new WaitForSeconds(0.35f);
        bottomBanner.SetActive(true);
        yield return new WaitForEndOfFrame();

        MobileNativeShare.ShareScreenshot("screenshot", "");
        bottomBanner.SetActive(false);
    }
}
