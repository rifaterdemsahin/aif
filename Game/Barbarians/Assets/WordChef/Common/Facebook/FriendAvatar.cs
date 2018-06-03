using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FriendAvatar : Photo
{
    public int index;

    private void Start()
    {
        SetSize(100, 100);
        SetRealSize(CommonConst.FACE_AVATAR_SIZE, CommonConst.FACE_AVATAR_SIZE);
        if (CGameState.friendAvatars[index] != null)
            SetPhoto(CGameState.friendAvatars[index]);
        else
            Load();
    }

    protected override void OnPhotoLoaded(Sprite sprite)
    {
        base.OnPhotoLoaded(sprite);
        CGameState.friendAvatars[index] = sprite;
    }
}
