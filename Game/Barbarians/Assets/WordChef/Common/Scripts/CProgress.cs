using UnityEngine;

public class CProgress : MonoBehaviour {
    public float[] targetsPercent;
    public float maxProgress = 1000;
    public GameObject[] stars;
 
    private bool[] reachTargets;
    private RectTransform rectransform;
    private float maxWidth;

    private float current;
    public float Current
    {
        get { return current; }
        set
        {
            current = value;
            UpdateUI();
        }
    }

    private void Start()
    {
        rectransform = GetComponent<RectTransform>();
        maxWidth = rectransform.sizeDelta.x;
        reachTargets = new bool[targetsPercent.Length];
        Current = 0;
    }

    private void UpdateUI()
    {
        if (maxProgress == 0) return;

        float progress = Mathf.Clamp(Current / maxProgress, 0, 1);
        rectransform.sizeDelta = new Vector2(maxWidth * progress, rectransform.sizeDelta.y);

        for (int i = 0; i < targetsPercent.Length; i++)
        {
            if (reachTargets[i] == false && progress >= targetsPercent[i])
            {
                OnReachTarget(i);
                reachTargets[i] = true;
            }
        }
    }

    private void OnReachTarget(int target)
    {
        if (stars.Length > target)
            stars[target].SetActive(true);
    }

    public int GetReachedTarget()
    {
        int count = 0;
        for (int i = 0; i < reachTargets.Length; i++)
        {
            if (reachTargets[i]) count++;
        }
        return count;
    }
}
