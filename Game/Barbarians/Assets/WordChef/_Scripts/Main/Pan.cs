using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Text;

public class Pan : MonoBehaviour
{
    private int numLetters;
    private string word, panWord;
    private GameLevel gameLevel;
    private const float RADIUS = 100;
    private List<Vector3> letterPositions = new List<Vector3>();
    private List<Vector3> letterLocalPositions = new List<Vector3>();
    private List<Text> letterTexts = new List<Text>();
    private List<int> indexes = new List<int>();

    private int world, subWorld, level;

    public Transform centerPoint;
    public TextPreview textPreview;

    public static Pan instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;
    }

    public void Load(GameLevel gameLevel)
    {
        this.gameLevel = gameLevel;
        numLetters = gameLevel.word.Trim().Length;

        float delta = 360f / numLetters;

        float angle = 90;
        for (int i = 0; i < numLetters; i++)
        {
            float angleRadian = angle * Mathf.PI / 180f;
            float x = Mathf.Cos(angleRadian);
            float y = Mathf.Sin(angleRadian);
            Vector3 position = RADIUS * new Vector3(x, y, 0);

            letterLocalPositions.Add(position);
            letterPositions.Add(centerPoint.TransformPoint(position));

            angle += delta;
        }

        LineDrawer.instance.letterPositions = letterPositions;

        for(int i = 0; i < numLetters; i++)
        {
            Text letter = Instantiate(MonoUtils.instance.letter);
            letter.transform.SetParent(centerPoint);
            letter.transform.localScale = Vector3.one;
            letter.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10)));
            letter.text = gameLevel.word[i].ToString().ToUpper();
            letter.fontSize = ConfigController.Config.fontSizeInDiskMainScene;
            letterTexts.Add(letter);
        }

        indexes = Prefs.GetPanWordIndexes(world, subWorld, level).ToList();
        if (indexes.Count != numLetters)
        {
            indexes = Enumerable.Range(0, numLetters).ToList();
            indexes.Shuffle(level);
            Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());
        }

        GetPanWord();

        for (int i = 0; i < numLetters; i++)
        {
            letterTexts[i].transform.localPosition = letterLocalPositions[indexes.IndexOf(i)];
        }
    }

    private void GetShuffeWord()
    {
        List<int> origin = new List<int>();
        origin.AddRange(indexes);
        while (true)
        {
            indexes.Shuffle();
            if (!origin.SequenceEqual(indexes)) break;
        }
        GetPanWord();
    }

    private void GetPanWord()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < numLetters; i++)
        {
            sb.Append(gameLevel.word[indexes[i]]);
        }
        panWord = sb.ToString();
        textPreview.word = panWord.ToUpper();
    }

    public void Shuffle()
    {
        GetShuffeWord();
        Prefs.SetPanWordIndexes(world, subWorld, level, indexes.ToArray());

        int i = 0;
        foreach(var text in letterTexts)
        {
            iTween.MoveTo(text.gameObject, iTween.Hash("position", letterLocalPositions[indexes.IndexOf(i)], "time", 0.15f, "isLocal", true));
            text.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-10, 10)));
            i++;
        }
        Sound.instance.PlayButton();
    }
}
