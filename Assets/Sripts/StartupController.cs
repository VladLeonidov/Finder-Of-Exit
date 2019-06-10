using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour
{
    [SerializeField]
    private Slider progressBar;

    private void Awake()
    {
        Messenger<int, int>.AddListener(StartupEvent.MANAGERS_PROGRESS, OnManagerProgress);
        Messenger.AddListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnDestroy()
    {
        Messenger<int, int>.RemoveListener(StartupEvent.MANAGERS_PROGRESS, OnManagerProgress);
        Messenger.RemoveListener(StartupEvent.MANAGERS_STARTED, OnManagersStarted);
    }

    private void OnManagerProgress(int numReady, int numModules)
    {
        float progress = (float) numReady / numModules;
        progressBar.value = progress;
    }

    private void OnManagersStarted()
    {
        ManagersProvider.Mission.GoToNextLevel();
    }
}