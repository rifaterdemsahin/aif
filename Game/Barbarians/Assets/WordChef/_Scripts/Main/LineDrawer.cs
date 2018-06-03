using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour {

    public List<Vector3> letterPositions = new List<Vector3>();
    public List<int> currentIndexes = new List<int>();
    public List<Vector3> points = new List<Vector3>();
    public List<Vector3> positions = new List<Vector3>();
    public TextPreview textPreview;

    private LineRenderer lineRenderer;
    private Vector3 mousePoint;
    public static LineDrawer instance;

    private bool isDragging;
    private float RADIUS = 0.35f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.sortingLayerName = "MyLineRenderer";
    }

    private void Update()
    {
        if (DialogController.instance.IsDialogShowing()) return;
        if (SocialRegion.instance.isShowing) return;

        if (Input.GetMouseButtonDown(0))
        {
            textPreview.SetText("");
            textPreview.FadeIn();
        }

        if (Input.GetMouseButton(0))
        {
            isDragging = true;
            textPreview.SetActive(true);

            mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 90;

            int nearest = GetNearestPosition(mousePoint, letterPositions);
            Vector3 letterPosition = letterPositions[nearest];
            if (Vector3.Distance(letterPosition, mousePoint) < RADIUS)
            {
                if (currentIndexes.Count >= 2 && currentIndexes[currentIndexes.Count - 2] == nearest)
                {
                    currentIndexes.RemoveAt(currentIndexes.Count - 1);
                    textPreview.SetIndexes(currentIndexes);
                }
                else if (!currentIndexes.Contains(nearest))
                {
                    currentIndexes.Add(nearest);
                    textPreview.SetIndexes(currentIndexes);
                }
            }

            BuildPoints();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            currentIndexes.Clear();
#if UNITY_5_5
            lineRenderer.numPositions = 0;
#else
            lineRenderer.positionCount = 0;
#endif

            WordRegion.instance.CheckAnswer(textPreview.GetText());
        }

        if (points.Count >= 2 && isDragging)
        {
            positions = iTween.GetSmoothPoints(points.ToArray(), 8);
#if UNITY_5_5
            lineRenderer.numPositions = positions.Count;
#else
            lineRenderer.positionCount = positions.Count;
#endif
            lineRenderer.SetPositions(positions.ToArray());
        }
    }

    private int GetNearestPosition(Vector3 point, List<Vector3> letters)
    {
        float min = float.MaxValue;
        int index = -1;
        for(int i = 0; i < letters.Count; i++)
        {
            float distant = Vector3.Distance(point, letters[i]);
            if (distant < min)
            {
                min = distant;
                index = i;
            }
        }
        return index;
    }

    private void BuildPoints()
    {
        points.Clear();
        foreach (var i in currentIndexes) points.Add(letterPositions[i]);

        if (currentIndexes.Count == 1 || points.Count >= 1 && Vector3.Distance(mousePoint, points[points.Count - 1]) >= RADIUS)
        {
            points.Add(mousePoint);
        }
    }
}
