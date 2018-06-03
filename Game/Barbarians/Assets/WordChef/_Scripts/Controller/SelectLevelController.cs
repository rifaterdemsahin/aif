using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelController : BaseController {
    public RectTransform mainUI, scrollContent;
    public ScrollRect scrollRect;

    protected override void Start()
    {
        base.Start();

        int numLevels = Superpow.Utils.GetNumLevels(GameState.currentWorld, GameState.currentSubWorld);
        for(int i =0; i < numLevels; i++)
        {
            GameObject levelButton = Instantiate(MonoUtils.instance.levelButton);
            levelButton.transform.SetParent(scrollContent);
            levelButton.transform.localScale = Vector3.one;
            levelButton.transform.SetLocalZ(0);
        }

		StartCoroutine(UpdateGrid());
    }

	private IEnumerator UpdateGrid()
	{
		yield return new WaitForEndOfFrame();
		if (GameState.currentWorld == Prefs.unlockedWorld && GameState.currentSubWorld == Prefs.unlockedSubWorld)
		{
			Transform unlockedLevelTransform = scrollContent.transform.GetChild(Prefs.unlockedLevel);
			float newY = -unlockedLevelTransform.localPosition.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y / 2f;
			newY = Mathf.Clamp(newY, 0, scrollContent.sizeDelta.y);
			scrollContent.localPosition = new Vector3(scrollContent.localPosition.x, newY, scrollContent.localPosition.z);
		}
	}
}
