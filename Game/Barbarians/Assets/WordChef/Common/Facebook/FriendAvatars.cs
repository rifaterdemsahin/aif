using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FriendAvatars : MonoBehaviour {
    public GameObject friendAvatarPrefab;
    public Transform levelsTransform;

    private void Start()
    {
        FacebookUtils.ChangeFriendLevels();

        string avatarUrls = FacebookUtils.friendAvatarUrls;
        string friendLevels = FacebookUtils.friendLevels;
        if (avatarUrls == "" || friendLevels == "") return;

        List<string> avatarUrlList = CUtils.BuildListFromString<string>(avatarUrls);
        List<int> friendLevelList = CUtils.BuildListFromString<int>(friendLevels);

        int i = 0;
        int[] sumFriendInLevels = new int[CommonConst.TOTAL_LEVELS];
        foreach (int level in friendLevelList)
        {
                sumFriendInLevels[level - 1]++;
        }
        int[] countFriendInLevels = new int[CommonConst.TOTAL_LEVELS];
        foreach (string avatarUrl in avatarUrlList)
        {
            if (i >= friendLevelList.Count) break;
            int level = friendLevelList[i];
            if (level <= CommonConst.TOTAL_LEVELS)
            {
                //Debug.Log(avatarUrl);
                Transform levelTransform = levelsTransform.GetChild(level - 1);

                GameObject friendAvatar = (GameObject)Instantiate(friendAvatarPrefab);
                friendAvatar.transform.SetParent(transform);
                friendAvatar.transform.position = levelTransform.position + new Vector3(0.1f * (-(sumFriendInLevels[level - 1] - 1) / 2f + countFriendInLevels[level - 1]), 0);
                friendAvatar.transform.localScale = Vector3.one;
                FriendAvatar fAvatar = friendAvatar.GetComponent<FriendAvatar>();
                fAvatar.url = avatarUrl;
                fAvatar.index = i;
                countFriendInLevels[level - 1]++;
            }
            i++;
        }
    }
}
