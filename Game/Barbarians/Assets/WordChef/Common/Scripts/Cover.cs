using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cover : MonoBehaviour
{
    public RectTransform left, right, above, below;
    
    private void Start()
    {
        UpdateCover();
        UICamera.instance.onScreenSizeChanged += UpdateCover;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        UpdateCover();
        UICamera.instance.onScreenSizeChanged += UpdateCover;
    }

    private void UpdateCover()
    {
        float width = UICamera.instance.GetWidth();
        float height = UICamera.instance.GetHeight();
        float screenWidth = UICamera.instance.virtualWidth;
        float screenHeight = UICamera.instance.virtualHeight;

        float paddingLeftRight = (screenWidth - width * 200) / 2;
        float paddingAboveBelow = (screenHeight - height * 200) / 2;

        left.gameObject.SetActive(paddingLeftRight > 0.0001f);
        right.gameObject.SetActive(paddingLeftRight > 0.0001f);

        above.gameObject.SetActive(paddingAboveBelow > 0.0001f);
        below.gameObject.SetActive(paddingAboveBelow > 0.0001f);

        float leftWidth = UICamera.instance.landscape ? 400 : 800;
        float aboveHeight = UICamera.instance.landscape ? 800 : 400;

        if (left.sizeDelta.x < paddingLeftRight)
        {
            left.sizeDelta = new Vector2(paddingLeftRight, left.sizeDelta.y);
            right.sizeDelta = new Vector2(paddingLeftRight, right.sizeDelta.y);
        }
        else
        {
            left.sizeDelta = new Vector2(leftWidth, left.sizeDelta.y);
            right.sizeDelta = new Vector2(leftWidth, right.sizeDelta.y);
        }

        if (above.sizeDelta.y < paddingAboveBelow)
        {
            above.sizeDelta = new Vector2(above.sizeDelta.x, paddingAboveBelow);
            below.sizeDelta = new Vector2(below.sizeDelta.x, paddingAboveBelow);
        }
        else
        {
            above.sizeDelta = new Vector2(above.sizeDelta.x, aboveHeight);
            below.sizeDelta = new Vector2(below.sizeDelta.x, aboveHeight);
        }
    }
}
