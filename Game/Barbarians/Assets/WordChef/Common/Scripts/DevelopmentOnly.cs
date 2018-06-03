using UnityEngine;
using System.Collections;
using System.IO;

public class DevelopmentOnly : MonoBehaviour {
    public bool setRuby;
    public int ruby;

    public bool unlockAllLevels;

    public bool clearAllPlayerPrefs;

    private void Start()
    {
        if (setRuby)
            CurrencyController.SetBalance(ruby);

        if (unlockAllLevels)
        {
            Prefs.unlockedWorld = 5;
            Prefs.unlockedSubWorld = 5;
            Prefs.unlockedLevel = 18;
        }

        if (clearAllPlayerPrefs)
        {
            CPlayerPrefs.DeleteAll();
            CPlayerPrefs.Save();
        }
    }
}
