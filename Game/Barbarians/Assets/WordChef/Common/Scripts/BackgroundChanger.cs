using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    public Image image;
    public Sprite[] sprites;
    public int[] levelIndexes;

    private void Start()
    {
        int level = LevelController.GetCurrentLevel();
        image.sprite = sprites[GetLevelIndex(level)];
    }

    private int GetLevelIndex(int level)
    {
        for (int i = levelIndexes.Length - 1 ; i >= 0; i--)
        {
            if (level >= levelIndexes[i]) return i;
        }
        return 0;
    }
}
