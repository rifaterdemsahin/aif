using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDialog : Dialog {
    public Animator titleAnim;
    private int numLevels;
    private bool isLastLevel;
    private int subWorld, level;

    protected override void Start()
    {
        base.Start();
        Invoke("ShowTitle", 0.35f);
        CheckUnlock();
    }

    private void ShowTitle()
    {
        titleAnim.SetTrigger("show");
    }

    private void CheckUnlock()
    {
        numLevels = Superpow.Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;

        isLastLevel = Prefs.IsLastLevel();

        GameState.currentLevel = (level + 1) % numLevels;
        if (level == numLevels - 1)
        {
            GameState.currentSubWorld = (subWorld + 1) % Const.NUM_SUBWORLD;
            if (subWorld == Const.NUM_SUBWORLD - 1)
            {
                GameState.currentWorld++;
            }
        }

        if (isLastLevel)
        {
            Prefs.unlockedWorld = GameState.currentWorld;
            Prefs.unlockedSubWorld = GameState.currentSubWorld;
            Prefs.unlockedLevel = GameState.currentLevel;

            int score = Superpow.Utils.GetLeaderboardScore();
            GPGSController.instance.ReportScore(score);
        }
    }

    public void NextClick()
    {
        Close();
        Sound.instance.PlayButton();

        CUtils.LoadScene(level == numLevels - 1 ? 1 : 3, true);
    }

    public void LeaderboardClick()
    {
        Sound.instance.PlayButton();

        int score = Superpow.Utils.GetLeaderboardScore();
        GPGSController.instance.ReportScoreAndShowLeaderboard(score);
    }
}
