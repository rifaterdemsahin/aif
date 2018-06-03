using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class TextPreview : MonoBehaviour {
    public GameObject content;
    public RectTransform backgroundRT, textRT;
    public string word;
    public Text text;

    [Header("")]
    public Color answerColor;
    public Color validColor;
    public Color wrongColor;
    public Color existColor;
    public Color defaultColor;

    public static TextPreview instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetIndexes(List<int> indexes)
    {
        StringBuilder sb = new StringBuilder();
        foreach(var i in indexes)
        {
            sb.Append(word[i]);
        }
        text.text = sb.ToString();
        backgroundRT.sizeDelta = new Vector2(textRT.GetComponent<Text>().preferredWidth + 50, backgroundRT.sizeDelta.y);
    }

    public void SetActive(bool isActive)
    {
        content.SetActive(isActive && text.text.Length > 0);
    }

    public void ClearText()
    {
        text.text = "";
    }

    public void SetText(string textStr)
    {
        text.text = textStr;
    }

    public string GetText()
    {
        return text.text;
    }

    public void SetAnswerColor()
    {
        backgroundRT.GetComponent<Image>().color = answerColor;
    }

    public void SetValidColor()
    {
        backgroundRT.GetComponent<Image>().color = validColor;
    }

    public void SetWrongColor()
    {
        backgroundRT.GetComponent<Image>().color = wrongColor;
    }

    public void SetDefaultColor()
    {
        backgroundRT.GetComponent<Image>().color = defaultColor;
    }

    public void SetExistColor()
    {
        backgroundRT.GetComponent<Image>().color = existColor;
    }

    public void FadeOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", 0.4f, "onupdate", "OnUpdate", "ontarget", gameObject));
    }

    public void FadeIn()
    {
        iTween.Stop(gameObject);
        SetDefaultColor();
        content.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void OnUpdate(float value)
    {
        content.GetComponent<CanvasGroup>().alpha = value;
    }
}
