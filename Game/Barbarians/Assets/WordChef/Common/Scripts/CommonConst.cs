//#define DEVELOPMENT
public class CommonConst
{
    public const iTween.DimensionMode ITWEEN_MODE = iTween.DimensionMode.mode2D;

    public static readonly int[] START_FRIEND_LEVELS = { 3, 5, 7, 12, 18 };
    public static int GetTargetScore(int level)
    {
        return 1000;
    }

#if DEVELOPMENT
    public const int MIN_INVITE_FRIEND = 1;
    public const int MAX_INVITE_FRIEND = 20;
    public const bool ENCRYPTION_PREFS = false;
    public const int MIN_LEVEL_TO_RATE = 1;
#else
    public const int MIN_INVITE_FRIEND = 40;
    public const int MAX_INVITE_FRIEND = 50;
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
    public const bool ENCRYPTION_PREFS = true;
#else
    public const bool ENCRYPTION_PREFS = false;
#endif
    public const int MIN_LEVEL_TO_RATE = 3;
#endif

    public const int MAX_FRIEND_IN_MAP = 15;
    public const int FACE_AVATAR_SIZE = 100;

    public const int TOTAL_LEVELS = 50;
}
