using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.SimpleAndroidNotifications;
using System;
using Superpow;
using System.Text;

public class NotificationController : MonoBehaviour {

    private const double NOTI_DELAY_1 = 1 * 24;
    private const double NOTI_DELAY_2 = 2 * 24;
    private const double NOTI_DELAY_3 = 4 * 24;
    private const double NOTI_DELAY_4 = 7 * 24;
    private const double NOTI_DELAY_5 = 10 * 24;
    private const double NOTI_DELAY_6 = 15 * 24;
    private const double NOTI_DELAY_7 = 30 * 24;

    private bool ingame = false;

    private void Start()
    {
        InGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            OutGame();
        else
            InGame();
    }

    private void OnApplicationQuit()
    {
        OutGame();
    }

    private void InGame()
    {
        if (ingame) return;
        ingame = true;

        if (!PlayerPrefs.HasKey("out_game_time")) return;

        double restTimeSecs = CUtils.GetCurrentTime() - CUtils.GetActionTime("out_game");
        double restTime = restTimeSecs / 3600;

        if (PlayerPrefs.HasKey("level_progress"))
        {
            if (Prefs.isNoti1Enabled && restTime > NOTI_DELAY_1)
            {
                UnlockLetter();
            }

            if (Prefs.isNoti2Enabled && restTime > NOTI_DELAY_2)
            {
                UnlockLetter();
            }
        }

        int num = 0;

        if (restTime > NOTI_DELAY_3)
        {
            GiveFreeRuby(Prefs.noti3Ruby);
            num += Prefs.noti3Ruby;
        }

        if (restTime > NOTI_DELAY_4)
        {
            GiveFreeRuby(Prefs.noti4Ruby);
            num += Prefs.noti4Ruby;
        }

        if (restTime > NOTI_DELAY_5)
        {
            GiveFreeRuby(Prefs.noti5Ruby);
            num += Prefs.noti5Ruby;
        }

        if (restTime > NOTI_DELAY_6)
        {
            GiveFreeRuby(Prefs.noti6Ruby);
            num += Prefs.noti6Ruby;
        }

        if (restTime > NOTI_DELAY_7)
        {
            GiveFreeRuby(Prefs.noti7Ruby);
            num += Prefs.noti7Ruby;
        }

        if (num > 0)
        {
            Toast.instance.ShowMessage(string.Format("You got {0} free rubies", num));
        }

        Prefs.isNoti1Enabled = false;
        Prefs.isNoti2Enabled = false;
        NotificationManager.CancelAll();
    }

    private void OutGame()
    {
        ingame = false;

        if (PlayerPrefs.HasKey("level_progress"))
        {
            char? letter = GetUnlockLetter(0);
            if (letter != null)
            {
                PushUnlockLetter((char)letter, NOTI_DELAY_1);
                Prefs.isNoti1Enabled = true;
            }

            letter = GetUnlockLetter(1);
            if (letter != null)
            {
                PushUnlockLetter((char)letter, NOTI_DELAY_2);
                Prefs.isNoti2Enabled = true;
            }
        }

        int num = PushFreeRuby(NOTI_DELAY_3);
        Prefs.noti3Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_4);
        Prefs.noti4Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_5);
        Prefs.noti5Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_6);
        Prefs.noti6Ruby = num;

        num = PushFreeRuby(NOTI_DELAY_7);
        Prefs.noti7Ruby = num;

        CUtils.SetActionTime("out_game");
    }

    private TwoValues<int, int> GetUnlockLetterIndex(string[] progress, int numPass = 0)
    {
        int numEmpty = 0;
        int tmpNumPass = numPass;
        TwoValues<int, int> t = null;

        for (int i = 0; i < progress.Length; i++)
        {
            for (int j = 0; j < progress[i].Length; j++)
            {
                if (progress[i][j] == '0')
                {
                    if (t == null)
                    {
                        if (tmpNumPass == 0)
                            t = new TwoValues<int, int>(i, j);
                        else
                            tmpNumPass--;
                    }

                    numEmpty++;
                }
            }
        }

        if (numEmpty - numPass >= 2)
            return t;
        else
            return null;
    }

    private char? GetUnlockLetter(int numPass)
    {
        string[] progress = Prefs.levelProgress;
        GameLevel gameLevel = Utils.Load(Prefs.unlockedWorld, Prefs.unlockedSubWorld, Prefs.unlockedLevel);
        var answers = CUtils.BuildListFromString<string>(gameLevel.answers);

        var index = GetUnlockLetterIndex(progress, numPass);
        if (index != null && index.Item1 < answers.Count)
        {
            if (index.Item2 < answers[index.Item1].Length)
            {
                char letter = answers[index.Item1][index.Item2];
                letter = char.ToUpper(letter);
                return letter;
            }
            else return null;
        }
        return null;
    }

    private void UnlockLetter()
    {
        string[] progress = Prefs.levelProgress;
        var index = GetUnlockLetterIndex(progress);

        if (index == null) return;
        if (index.Item1 >= progress.Length) return;

        StringBuilder sb = new StringBuilder(progress[index.Item1]);
        sb[index.Item2] = '1';
        progress[index.Item1] = sb.ToString();

        Prefs.levelProgress = progress;
    }

    private void GiveFreeRuby(int num)
    {
        CurrencyController.CreditBalance(num);
    }

    private void PushUnlockLetter(char letter, double delay)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string message = "We unlocked a letter " + letter + " for you. Open the game and continue :)";
        NotificationManager.SendWithAppIcon(TimeSpan.FromHours(delay), "Unlock letter", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
    }

    private int PushFreeRuby(double delay)
    {
        int num = UnityEngine.Random.Range(5, 8);
#if UNITY_ANDROID && !UNITY_EDITOR
        string message = "You got " + num + " free rubies. Let's play and challenge yourself";
        NotificationManager.SendWithAppIcon(TimeSpan.FromHours(delay), "Ruby gift", message, new Color(0, 0.6f, 1), NotificationIcon.Message);
#endif
        return num;
    }
}
