using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FacebookUtils
{
    public const string INVITABLE_OFFSET = "invitable_offset";
    public static int invitableOffset
    {
        get { return PlayerPrefs.GetInt(INVITABLE_OFFSET, 0); }
        set { PlayerPrefs.SetInt(INVITABLE_OFFSET, value); }
    }

    public static string permissions
    {
        get { return PlayerPrefs.GetString("facebook_permissions", ""); }
        set { PlayerPrefs.SetString("facebook_permissions", value); }
    }

    public static bool HasPublishActions()
    {
        return permissions.Contains("publish_actions");
    }

    public static string friendAvatarUrls
    {
        get { return PlayerPrefs.GetString("facebook_friends_avatar_urls", ""); }
        set { PlayerPrefs.SetString("facebook_friends_avatar_urls", value); }
    }

    public static string myAvatarUrl
    {
        get { return PlayerPrefs.GetString("facebook_my_avatar_url", ""); }
        set { PlayerPrefs.SetString("facebook_my_avatar_url", value); }
    }

    public static string GetFriendAvatarUrl(int index)
    {
        List<string> avatarUrls = CUtils.BuildListFromString<string>(friendAvatarUrls);
        if (index < avatarUrls.Count)
        {
            return avatarUrls[index];
        }
        return "";
    }

    public static string friendLevels
    {
        get { return PlayerPrefs.GetString("facebook_friends_levels", ""); }
        set { PlayerPrefs.SetString("facebook_friends_levels", value); }
    }

    public static void SetFriendScores(int index, string scores)
    {
        PlayerPrefs.SetString("facebook_friends_scores_" + index, scores);
    }

    public static string GetFriendScores(int index)
    {
        return PlayerPrefs.GetString("facebook_friends_scores_" + index, "");
    }

    public static string myScores
    {
        get { return PlayerPrefs.GetString("facebook_my_scores", ""); }
        set { PlayerPrefs.SetString("facebook_my_scores", value); }
    }

    public static int GetMyScore(int level)
    {
        List<int> scores = CUtils.BuildListFromString<int>(myScores);
        if (level <= scores.Count)
        {
            return scores[level - 1];
        }
        return 0;
    }

    public static void SetMyScore(int level, int score)
    {
        List<int> scores = CUtils.BuildListFromString<int>(myScores);
        if (level <= scores.Count)
        {
            scores[level - 1] = score;
        }
        else if (level == scores.Count + 1)
        {
            scores.Add(score);
        }

        myScores = CUtils.BuildStringFromCollection(scores);
    }

    public static int GetFriendScore(int index, int level)
    {
        string strScores = GetFriendScores(index);
        List<int> scores = CUtils.BuildListFromString<int>(strScores);
        if (level <= scores.Count)
        {
            return scores[level - 1];
        }
        return 0;
    }

    public static List<int> GetFriendIndexesPassed(int level)
    {
        List<int> indexes = new List<int>();
        List<int> friendLevelList = CUtils.BuildListFromString<int>(friendLevels);
        for (int i = 0; i < friendLevelList.Count; i++)
        {
            if (friendLevelList[i] > level)
            {
                indexes.Add(i);
            }
        }
        return indexes;
    }

    public static void ChangeFriendLevels()
    {
        if (friendLevels == "") return;

        int dayChange = PlayerPrefs.GetInt("facebook_daychange_level", -1);
        if (DateTime.Now.DayOfYear == dayChange) return;

        PlayerPrefs.SetInt("facebook_daychange_level", DateTime.Now.DayOfYear);

        List<int> friendLevelList = CUtils.BuildListFromString<int>(friendLevels);
        for (int i = 0; i < friendLevelList.Count; i++)
        {
            int delta = UnityEngine.Random.Range(0, 3);
            friendLevelList[i] += delta;
            if (friendLevelList[i] >= CommonConst.TOTAL_LEVELS)
            {
                friendLevelList[i] = CommonConst.TOTAL_LEVELS;
            }
        }
        friendLevels = CUtils.BuildStringFromCollection(friendLevelList);
    }
}