using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DDOLCameraUI : MonoBehaviour {

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
        SetCamera();
    }

    private void SetCamera()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        GTil.Init(this);
    }
}
