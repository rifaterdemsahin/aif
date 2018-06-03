using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PromoteDialog : Dialog{
    public Image feature;
    public Text promoteMessage;

    [HideInInspector]
    public Promote promote;

    protected override void Start()
    {
        base.Start();

        promote = PromoteController.instance.GetPromote(PromoteType.PopupDialog);
        StartCoroutine(CUtils.LoadPicture(promote.featureUrl, LoadPictureComplete, promote.featureWidth, promote.featureHeight));

        if (!string.IsNullOrEmpty(promote.message))
        {
            promoteMessage.text = promote.message;
            RectTransform parent = promoteMessage.transform.parent.GetComponent<RectTransform>();
            parent.sizeDelta = new Vector2(promoteMessage.preferredWidth + 30, parent.sizeDelta.y);
            if (promote.rewardType == RewardType.RemoveAds && (CUtils.IsBuyItem() || CUtils.IsAdsRemoved()))
            {
                HideText();
            }
        }
        else
        {
            HideText();
        }
    }

    public void OnInstallClick()
    {
        CUtils.OpenStore(promote.package);
        PromoteController.instance.OnInstallClick(promote.package);

        Sound.instance.PlayButton();
        Close();
    }

    public void LaterClick()
    {
        Sound.instance.PlayButton();
        Close();
    }

    private void HideText()
    {
        promoteMessage.transform.parent.gameObject.SetActive(false);
    }

    private void LoadPictureComplete(Texture2D texture)
    {
        feature.sprite = CUtils.CreateSprite(texture, promote.featureWidth, promote.featureHeight);
    }
}
