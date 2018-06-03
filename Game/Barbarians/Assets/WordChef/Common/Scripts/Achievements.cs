using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Superpow;
using System;

public class Achievements
{
    public List<Achievement> achievements;
    public int maxShown = int.MaxValue;
    public Action onAchievementComplete;
    public Action onRewardReceived;
    protected int lastIndex;

    public Achievements()
    {
        achievements = new List<Achievement>();
    }

    public void Increasement(string id, int step)
    {
        int index;
        Achievement achievement = GetAchievement(id, out index);
        achievement.currentStep = Mathf.Clamp(achievement.currentStep + step, 0, achievement.totalStep);
        if (achievement.currentStep == achievement.totalStep)
        {
            achievement.isUnlocked = true;
            if (onAchievementComplete != null)
            {
                onAchievementComplete();
            }
        }
        achievements[index] = achievement;
        SaveAchievement(achievement);
    }

    public Achievement GetAchievement(string id)
    {
        return achievements.Find(x => x.id == id);
    }

    public Achievement GetAchievement(string id, out int index)
    {
        index = -1;
        for (int i = 0; i < achievements.Count; i++)
        {
            if (achievements[i].id == id)
            {
                index = i;
                return achievements[i];
            }
        }
        return null;
    }

    public Achievement GetAchievementFromPref(string id)
    {
        string json = CPlayerPrefs.GetString("achievement_" + id);
        if (string.IsNullOrEmpty(json)) return null;

        return ParseAchievement(json);
    }

    public Achievement ParseAchievement(string json)
    {
        JSONNode N = JSON.Parse(json);
        return ParseAchievement(N);
    }

    public Achievement ParseAchievement(JSONNode N)
    {
        Achievement achievement = new Achievement();
        achievement.id = N["id"];
        achievement.description = N["description"];
        achievement.icon = N["icon"];
        achievement.isUnlocked = N["isUnlocked"].AsBool;
        achievement.totalStep = N["totalStep"] != null ? N["totalStep"].AsInt : 1;
        achievement.currentStep = N["currentStep"] != null ? N["currentStep"].AsInt : 0;
        achievement.type = N["type"];

        Reward reward = new Reward();
        reward.type = N["reward_type"];
        reward.number = N["reward_number"].AsInt;
        reward.received = N["isRewardReceived"].AsBool;

        achievement.reward = reward;

        return achievement;
    }

    public List<Achievement> GetAchievementsToShow()
    {
        List<Achievement> matches = achievements.FindAll(x => x.reward.received == false);
        int numofShow = Mathf.Min(maxShown, matches.Count);
        return matches.GetRange(0, numofShow);
    }

    public virtual void ReceiveReward(string achivementID)
    {
        int index;
        Achievement ach = GetAchievement(achivementID, out index);
        achievements[index].reward.received = true;
        Reward(ach);
        if (onRewardReceived != null) onRewardReceived();
    }

    protected virtual void Reward(Achievement ch)
    {

    }

    protected virtual void Load(string json)
    {
        achievements.Clear();
        JSONArray arr = JSON.Parse(json).AsArray;
        foreach (JSONNode N in arr)
        {
            Achievement achievement = ParseAchievement(N);
            achievements.Add(achievement);
        }
    }

    public void LoadFile(string filePath)
    {
        string json = CUtils.ReadFileContent(filePath);
        Load(json);
    }

    public void UpdateData()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            Achievement ach = achievements[i];
            string key = "achievement_" + ach.id;
            if (CPlayerPrefs.HasKey(key))
            {
                JSONNode N = JSON.Parse(CPlayerPrefs.GetString(key));
                ach.isUnlocked = N["isUnlocked"].AsBool;
                ach.reward.received = N["isRewardReceived"].AsBool;
                ach.currentStep = N["currentStep"].AsInt;

                achievements[i] = ach;
            }
        }
    }

    public void SaveAchievement(string id)
    {
        int index;
        Achievement ach = GetAchievement(id, out index);
        SaveAchievement(ach);
    }

    public void SaveAchievement(Achievement ach)
    {
        JSONClass cl = new JSONClass();
        cl["id"] = ach.id;
        cl["description"] = ach.description;
        cl["icon"] = ach.icon;
        cl["isUnlocked"].AsBool = ach.isUnlocked;
        cl["type"] = ach.type;
        cl["currentStep"].AsInt = ach.currentStep;
        cl["totalStep"].AsInt = ach.totalStep;
        cl["reward_type"] = ach.reward.type;
        cl["reward_number"].AsInt = ach.reward.number;
        cl["isRewardReceived"].AsBool = ach.reward.received;

        CPlayerPrefs.SetString("achievement_" + ach.id, cl.ToString());
        CPlayerPrefs.Save();
    }

    public void SaveAllAchievements()
    {
        foreach (Achievement ach in achievements)
        {
            SaveAchievement(ach);
        }
    }

    protected virtual string BuildType(object type, object additionalParam)
    {
        return null;
    }

    protected virtual List<Achievement> GetMatchAchievement(string strType)
    {
        return null;
    }

    public void Check(object type, int step = 1, object additionalParam = null)
    {
        string strType = BuildType(type, additionalParam);
        List<Achievement> achs = GetMatchAchievement(strType);
        foreach (Achievement ach in achs)
        {
            if (ach != null)
            {
                Increasement(ach.id, step);
            }
        }
    }
}