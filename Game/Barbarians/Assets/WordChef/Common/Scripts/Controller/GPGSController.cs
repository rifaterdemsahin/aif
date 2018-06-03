//#define USE_GPGS

using UnityEngine;
using UnityCore;

#if USE_GPGS
using GooglePlayGames;
#endif

public class GPGSController : MonoBehaviour
{
    public static GPGSController instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
#if USE_GPGS
        PlayGamesPlatform.Activate();
#endif
    }

    public void ShowLeaderboard()
    {
#if USE_GPGS
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
                CFirebase.LogEvent("leaderboard", "show");
            }
        });

        CFirebase.LogEvent("leaderboard", "button_click");
#endif
    }

    public void ReportScore(int score)
    {
#if USE_GPGS
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, GPGSIds.leaderboard_high_score, (bool postSuccess) => {
                // handle success or failure
            });
        }
#endif
    }

    public void ReportScoreAndShowLeaderboard(int score)
    {
#if USE_GPGS
        Social.localUser.Authenticate((bool success) => 
        {
            if (success)
            {
                Social.ReportScore(score, GPGSIds.leaderboard_high_score, (bool postSuccess) => 
                {
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_high_score);
                    CFirebase.LogEvent("leaderboard", "show");
                });
            }
        });
#endif
    }
}