using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private void Awake()
    {
        ProfileManager.Init();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    void Start()
    {
        SceneManager.LoadScene(nextScene);
    }
}
