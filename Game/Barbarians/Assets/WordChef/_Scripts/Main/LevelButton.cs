using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public Text levelText;
    public int world, subWorld, level;
    public Transform centerPoint;
    public Image background;

    public Sprite solvedSprite, currentSprite, lockedSprite;

    private List<Vector3> letterLocalPositions = new List<Vector3>();
    private List<Text> letterTexts = new List<Text>();
    private GameLevel gameLevel;
    private const float RADIUS = 40f;

    public void Start()
    {
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = transform.GetSiblingIndex();
        levelText.text = (level + 1).ToString();
        GetComponent<Button>().onClick.AddListener(OnButtonClick);

        gameLevel = Resources.Load<GameLevel>("World_" + world + "/SubWorld_" + subWorld + "/Level_" + level);

        if (gameLevel != null)
        {
            Load();
        }

        int unlockedWorld = Prefs.unlockedWorld;
        int unlockedSubWorld = Prefs.unlockedSubWorld;
        int unlockedLevel = Prefs.unlockedLevel;

        if  (world < unlockedWorld || 
            (world == unlockedWorld && subWorld < unlockedSubWorld) || 
            (world == unlockedWorld && subWorld <= unlockedSubWorld && level < unlockedLevel))
        {
            background.sprite = solvedSprite;
        }
        else if (world == unlockedWorld && subWorld == unlockedSubWorld && level == unlockedLevel)
        {
            background.sprite = currentSprite;
        }
        else
        {
            background.sprite = lockedSprite;
            GetComponent<Button>().interactable = false;
        }
    }

    public void Load()
    {
        int numLetters = gameLevel.word.Trim().Length;
        if (numLetters == 0) return;

        float delta = 360f / numLetters;

        float angle = 90;
        for (int i = 0; i < numLetters; i++)
        {
            float angleRadian = angle * Mathf.PI / 180f;
            float x = Mathf.Cos(angleRadian);
            float y = Mathf.Sin(angleRadian);
            Vector3 position = RADIUS * new Vector3(x, y, 0);

            letterLocalPositions.Add(position);

            angle += delta;
        }

        for (int i = 0; i < numLetters; i++)
        {
            Text letter = Instantiate(MonoUtils.instance.letter);
            letter.transform.SetParent(centerPoint);
            letter.transform.localScale = Vector3.one;
            letter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15, 15)));
            letter.text = gameLevel.word[i].ToString().ToUpper();
            letter.fontSize = ConfigController.Config.fontSizeInDiskSelectLevel;
            letterTexts.Add(letter);
        }

        List<int> indexes = Prefs.GetPanWordIndexes(world, subWorld, level).ToList();
        if (indexes.Count != numLetters)
        {
            indexes = Enumerable.Range(0, numLetters).ToList();
            indexes.Shuffle(level);
            Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());
        }

        for (int i = 0; i < numLetters; i++)
        {
            letterTexts[i].transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
        }
    }

    public void OnButtonClick()
    {
        GameState.currentLevel = level;

        CUtils.LoadScene(3, true);
        Sound.instance.PlayButton();

        // Set the music
        Music.instance.Play(CUtils.GetRandom(Music.Type.Main_0, Music.Type.Main_1, Music.Type.Main_2));
    }
}
