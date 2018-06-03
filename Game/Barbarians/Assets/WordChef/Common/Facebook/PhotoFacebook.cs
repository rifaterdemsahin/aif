using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Facebook.Unity;

public class PhotoFacebook : Photo {
    public string facebookID;

    public PhotoFacebook()
    {
        realWidth = realHeight = CommonConst.FACE_AVATAR_SIZE;
    }

    public override void Load()
    {
        if (FB.IsLoggedIn)
        {
            if (facebookID == "me" && CGameState.userAvatar != null)
                SetPhoto(CGameState.userAvatar);
            else
            {
                string url = CUtils.GetFacePictureURL(facebookID, realWidth, realHeight);
                FacebookController.instance.LoadPictureAPI(url, MyPictureCallback, realWidth, realHeight, facebookID == "me");
            }
        }
        else
        {
            if (facebookID == "me")
            {
                string imageUrl = FacebookUtils.myAvatarUrl;
                if (!string.IsNullOrEmpty(imageUrl))
                    StartCoroutine(CUtils.LoadPicture(imageUrl, MyPictureCallback, realWidth, realHeight));
            }
        }
    }

    protected override void OnPhotoLoaded(Sprite sprite)
    {
        base.OnPhotoLoaded(sprite);

        if (facebookID == "me" && CGameState.userAvatar == null)
            CGameState.userAvatar = sprite;
    }
}
