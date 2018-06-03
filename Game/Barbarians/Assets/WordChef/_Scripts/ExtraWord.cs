using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class ExtraWord : MonoBehaviour {
    public List<string> extraWords = new List<string>();
    public GameObject existMessage;
    public Transform beginPoint, endPoint;
    public GameObject lightEffect, lightOpenEffect;

    private int world, subWorld, level;
    private CanvasGroup existMessageCG;
    private bool isMessageShowing;
    private Text flyText;
    
    public static ExtraWord instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        world = GameState.currentWorld;
        subWorld = GameState.currentSubWorld;
        level = GameState.currentLevel;

        extraWords = Prefs.GetExtraWords(world, subWorld, level).ToList();
        existMessage.SetActive(false);
        existMessageCG = existMessage.GetComponent<CanvasGroup>();

        UpdateUI();
    }

    private void UpdateUI()
    {
        lightOpenEffect.SetActive(Prefs.extraProgress >= Prefs.extraTarget);
    }

    public void ProcessWorld(string word)
    {
        if (extraWords.Contains(word))
        {
            if (isMessageShowing) return;
            isMessageShowing = true;

            ShowMessage("");
        }
        else
        {
            var middlePoint = CUtils.GetMiddlePoint(beginPoint.position, endPoint.position, 0.4f);
            Vector3[] waypoint = { beginPoint.position, middlePoint, endPoint.position };

            flyText = Instantiate(MonoUtils.instance.letter);
            flyText.text = word;
            flyText.fontSize = 12;
            flyText.transform.position = beginPoint.position;
            flyText.transform.SetParent(MonoUtils.instance.textFlyTransform);
            flyText.transform.localScale = TextPreview.instance.text.transform.localScale;
            iTween.MoveTo(flyText.gameObject, iTween.Hash("path", waypoint, "time", 0.3f, "oncomplete", "OnTextMoveToComplete", "oncompletetarget", gameObject));

            AddNewExtraWord(word);
        }
    }

    private void ShowMessage(string message)
    {
        existMessage.SetActive(true);
        existMessageCG.alpha = 0;
        iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", 1, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageShowComplete"));
    }

    public void AddNewExtraWord(string word)
    {
        extraWords.Add(word);
        Prefs.SetExtraWords(world, subWorld, level, extraWords.ToArray());
        Prefs.extraProgress++;
        Prefs.totalExtraAdded++;
    }

    private void OnMessageUpdate(float value)
    {
        existMessageCG.alpha = value;
    }

    private void OnMessageShowComplete()
    {
        Timer.Schedule(this, 0.5f, ()=>
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.3f, "OnUpdate", "OnMessageUpdate", "oncomplete", "OnMessageHideComplete"));
        });
    }

    private void OnMessageHideComplete()
    {
        isMessageShowing = false;
    }

    private void OnTextMoveToComplete()
    {
        UpdateUI();

        if (!lightOpenEffect.activeSelf)
        {
            lightEffect.SetActive(true);
            iTween.RotateAdd(lightEffect, iTween.Hash("z", -60, "time", 0.4f, "oncomplete", "OnLightRotateComplete", "oncompletetarget", gameObject));
        }

        flyText.CrossFadeAlpha(0, 0.3f, true);
        Destroy(flyText.gameObject, 0.3f);
    }

    private void OnLightRotateComplete()
    {
        lightEffect.SetActive(false);
    }

    public void OnClaimed()
    {
        UpdateUI();
    }
}
