using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixIssues : MonoBehaviour {

	private void Start()
	{
		FixIssue011();
	}

	private void FixIssue011()
	{
		bool isFixed = PlayerPrefs.GetInt("issue_011_fixed") == 1;
		if (!isFixed)
		{
			int[] worlds = {0, 0};
			int[] subworlds = {2,2};
			int[] levels = {9, 15};
			for(int i = 0; i < worlds.Length; i++)
			{
				if (Prefs.unlockedWorld == worlds[i] && Prefs.unlockedSubWorld == subworlds[i] && Prefs.unlockedLevel == levels[i])
				{
					PlayerPrefs.DeleteKey("level_progress");
				}
			}
			
			PlayerPrefs.SetInt("issue_011_fixed", 1);
		}
	}
}
