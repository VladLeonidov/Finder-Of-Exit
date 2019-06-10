using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour, IGameManager
{
    public ManagerStatus Status { get; private set; }

    public int CurrentLevel { get; private set; }

    public int MaxLevel { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service = null)
    {
        Debug.Log("Mission manager starting...");
        _network = service;

        UpdateData(0, 4);

        Status = ManagerStatus.Started;
    }

    public void GoToNextLevel()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
            ManagersProvider.Inventory.ResetInventory();
            string levelName = "Level" + CurrentLevel;
            Debug.Log("Loaing " + levelName);
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.Log("Last level");
            Messenger.Broadcast(GameEvent.GAME_COMPLETE);
            LoadMenuLevel();
        }
    }

    public void LoadMenuLevel()
    {
        CurrentLevel = 0;
        GoToNextLevel();
    }

    public void UpdateData(int curLevel, int maxLevel)
    {
        CurrentLevel = curLevel;
        MaxLevel = maxLevel;
    }

    public void ReachObjective()
    {
        Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
    }

    public void RestartCurrent(bool resetInventory)
    {
        if (resetInventory)
        {
            ManagersProvider.Inventory.ResetInventory();
        }
        string levelName = "Level" + CurrentLevel;
        Debug.Log("Loading " + levelName);
        SceneManager.LoadScene(levelName);
    }
}