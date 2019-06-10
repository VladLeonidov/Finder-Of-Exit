using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMenuScene();
        }
    }

    public void StartGame()
    {
        ManagersProvider.Mission.GoToNextLevel();
    }

    public void LoadMenuScene()
    {
        ManagersProvider.Mission.LoadMenuLevel();
    }

    public void LoadGame()
    {
        ManagersProvider.Data.LoadGameState();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}