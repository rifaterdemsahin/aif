using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class WordRegion : MonoBehaviour {
    public TextPreview textPreview;
    public Compliment compliment;

    private List<LineWord> lines = new List<LineWord>();
    private List<string> validWords = new List<string>();

    private GameLevel gameLevel;
    private int numWords, numCol, numRow;
    private float cellSize, startFirstColX;
    private bool hasLongLine;

    private RectTransform rt;
    public static WordRegion instance;

    private void Awake()
    {
        instance = this;
        rt = GetComponent<RectTransform>();
    }

    public void Load(GameLevel gameLevel)
    {
        this.gameLevel = gameLevel;

        var wordList = CUtils.BuildListFromString<string>(this.gameLevel.answers);
        validWords = CUtils.BuildListFromString<string>(this.gameLevel.validWords);
        numWords = wordList.Count;

        numCol = numWords <= 4 ? 1 :
                     numWords <= 10 ? 2 : 3;

        numRow = (int)Mathf.Ceil(numWords / (float)numCol);

        int maxCellInWidth = 0;
        
        for(int i = numRow; i <= numWords;  i += numRow)
        {
            maxCellInWidth += wordList[i - 1].Length;
        }

        if (numWords % numCol != 0) maxCellInWidth += wordList[numWords - 1].Length;

        if (numCol > 1)
        {
            float coef = (maxCellInWidth + (maxCellInWidth - numCol) * Const.CELL_GAP_COEF + (numCol - 1) * Const.COL_GAP_COEF);
            cellSize = rt.rect.width / coef;
            float maxSize = rt.rect.height / (numRow + (numRow + 1) * Const.CELL_GAP_COEF);
            if (maxSize < cellSize)
            {
                cellSize = maxSize;
                startFirstColX = (rt.rect.width - cellSize * coef) / 2f;
            }
        }
        else
        {
            cellSize = rt.rect.height / (numRow + (numRow - 1) * Const.CELL_GAP_COEF + 0.8f);
            float maxSize = rt.rect.width / (maxCellInWidth + (maxCellInWidth - 1) * Const.CELL_GAP_COEF);

            if (maxSize < cellSize)
            {
                hasLongLine = true;
                cellSize = maxSize;
            }
        }

        string[] levelProgress = GetLevelProgress();
        bool useProgress = false;

        if (levelProgress.Length != 0)
        {
            useProgress = CheckLevelProgress(levelProgress, wordList);
            if (!useProgress) ClearLevelProgress(); 
        }

        int lineIndex = 0;
        foreach (var word in wordList)
        {
            LineWord line = Instantiate(MonoUtils.instance.lineWord);
            line.answer = word.ToUpper();
            line.cellSize = cellSize;
            line.SetLineWidth();
            line.Build();

            if (useProgress)
            {
                line.SetProgress(levelProgress[lineIndex]);
            }

            line.transform.SetParent(transform);
            line.transform.localScale = Vector3.one;
            line.transform.localPosition = Vector3.zero;

            lines.Add(line);
            lineIndex++;
        }

        SetLinesPosition();
    }

    private void SetLinesPosition()
    {
        if (numCol >= 2)
        {
            float[] startX = new float[numCol];
            startX[0] = startFirstColX;

            for (int i = 1; i < numCol; i++)
            {
                startX[i] = startX[i - 1] + lines[numRow * i - 1].lineWidth + cellSize * Const.COL_GAP_COEF;
            }

            for (int i = 0; i < lines.Count; i++)
            {
                int lineX = i / numRow;
                int lineY = numRow - 1 - i % numRow;

                float x = startX[lineX];
                float gapY = (rt.rect.height - cellSize * numRow) / (numRow + 1);
                float y = (lineY + 1) * gapY + lineY * cellSize;

                lines[i].transform.localPosition = new Vector2(x, y);
            }
        }
        else
        {
            for (int i = 0; i < lines.Count; i++)
            {
                float x = rt.rect.width / 2 - lines[i].lineWidth / 2;
                float y;
                if (hasLongLine)
                {
                    float gapY = (rt.rect.height - numRow * cellSize) / (numRow + 1);
                    y = gapY + (cellSize + gapY) * (numRow - i - 1);
                }
                else
                {
                    y = 0.4f * cellSize + (cellSize + cellSize * Const.CELL_GAP_COEF) * (numRow - i - 1);
                }
                lines[i].transform.localPosition = new Vector2(x, y);
            }
        }
    }

    public void CheckAnswer(string checkWord)
    {
        LineWord line = lines.Find(x => x.answer == checkWord);
        if (line != null)
        {
            if (!line.isShown)
            {
                textPreview.SetAnswerColor();
                line.ShowAnswer();
                CheckGameComplete();

                if (lines.Last() == line)
                {
                    compliment.ShowRandom();
                }

                Sound.instance.Play(Sound.Others.Match);
            }
            else
            {
                textPreview.SetExistColor();
            }
        }
        else if (validWords.Contains(checkWord.ToLower()))
        {
            ExtraWord.instance.ProcessWorld(checkWord);
        }
        else
        {
            textPreview.SetWrongColor();
        }

        textPreview.FadeOut();
    }

    private void CheckGameComplete()
    {
        SaveLevelProgress();
        var isNotShown = lines.Find(x => !x.isShown);
        if (isNotShown == null)
        {
            ClearLevelProgress();
            MainController.instance.OnComplete();

            if (lines.Count >= 6)
            {
                compliment.ShowRandom();
            }
        }
    }

    public void HintClick()
    {
        int ballance = CurrencyController.GetBalance();
        if (ballance >= Const.HINT_COST)
        {
            var line = lines.Find(x => !x.isShown);

            if (line != null)
            {
                line.ShowHint();
                CurrencyController.DebitBalance(Const.HINT_COST);
                CheckGameComplete();

                Prefs.AddToNumHint(GameState.currentWorld, GameState.currentSubWorld, GameState.currentLevel);
            }
        }
        else
        {
            DialogController.instance.ShowDialog(DialogType.Shop);
        }
        Sound.instance.PlayButton();
    }

    public void SaveLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return;

        List<string> results = new List<string>();
        foreach(var line in lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var cell in line.cells)
            {
                sb.Append(cell.isShown ? "1" : "0");
            }
            results.Add(sb.ToString());
        }

        Prefs.levelProgress = results.ToArray();
    }

    public string[] GetLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return new string[0];
        return Prefs.levelProgress;
    }

    public void ClearLevelProgress()
    {
        if (!Prefs.IsLastLevel()) return;
        CPlayerPrefs.DeleteKey("level_progress");
    }

    public bool CheckLevelProgress(string[] levelProgress, List<string> wordList)
    {
        if (levelProgress.Length != wordList.Count) return false;

        for(int i = 0; i < wordList.Count; i++)
        {
            if (levelProgress[i].Length != wordList[i].Length) return false;
        }
        return true;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            Timer.Schedule(this, 0.5f, () =>
            {
                UpdateBoard();
            });
        }
    }

    private void UpdateBoard()
    {
        string[] progress = GetLevelProgress();
        if (progress.Length == 0) return;

        int i = 0;
        foreach(var line in lines)
        {
            line.SetProgress(progress[i]);
            i++;
        }
    }
}
