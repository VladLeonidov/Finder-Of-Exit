using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text healthLabel;
    [SerializeField]
    private Text levelEnding;
    [SerializeField]
    private Text itemsToCompleteLevel;
    [SerializeField]
    private InventoryPopup inventoryPopup;
    [SerializeField]
    private SettingsPopup settingsPopup;
    [SerializeField]
    private int oreToCompleteLevel = 3;

    private string _oreToComleteLevelText = "Ore need: ";

    private void Awake()
    {
        Messenger.AddListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.AddListener(GameEvent.LEVEL_COMPLETE, OnLevelComleted);
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.AddListener(GameEvent.GAME_COMPLETE, OnGameComplete);
        Messenger.AddListener(GameEvent.ORE_UPDATED, OnOreUpdated);

        itemsToCompleteLevel.text = _oreToComleteLevelText + oreToCompleteLevel;
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALTH_UPDATED, OnHealthUpdated);
        Messenger.RemoveListener(GameEvent.LEVEL_COMPLETE, OnLevelComleted);
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnLevelFailed);
        Messenger.RemoveListener(GameEvent.GAME_COMPLETE, OnGameComplete);
        Messenger.RemoveListener(GameEvent.ORE_UPDATED, OnOreUpdated);
    }

    private void Start()
    {
        OnHealthUpdated();
        levelEnding.gameObject.SetActive(false);
        inventoryPopup.gameObject.SetActive(false);
        settingsPopup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = settingsPopup.gameObject.activeSelf;
            settingsPopup.gameObject.SetActive(!isShowing);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isShowing = inventoryPopup.gameObject.activeSelf;
            inventoryPopup.gameObject.SetActive(!isShowing);
            inventoryPopup.Refresh();
        }
    }

    public void SaveGame()
    {
        ManagersProvider.Data.SaveGameState();
    }

    public void LoadGame()
    {
        ManagersProvider.Data.LoadGameState();
    }

    private void OnHealthUpdated()
    {
        string message = "Health: " + ManagersProvider.Player.CurrentHealth + "/" +
            ManagersProvider.Player.MaxHealth;
        healthLabel.text = message;
    }

    public void OnLevelFailed()
    {
        StartCoroutine(LevelFailed());
    }

    private IEnumerator LevelFailed()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Failed!";
        levelEnding.color = Color.red;

        yield return new WaitForSeconds(4);

        ManagersProvider.Player.PlayerRespawn();
        ManagersProvider.Mission.RestartCurrent(true);
    }

    private void OnGameComplete()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.color = Color.green;
        levelEnding.text = "You finished the game!!!";
    }

    private void OnLevelComleted()
    {
        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        levelEnding.gameObject.SetActive(true);
        levelEnding.text = "Level Complete!";
        levelEnding.color = Color.green;

        yield return new WaitForSeconds(2);

        ManagersProvider.Mission.GoToNextLevel();
    }

    private void OnOreUpdated()
    {
        --oreToCompleteLevel;
        if (oreToCompleteLevel < 0)
        {
            oreToCompleteLevel = 0;
        }

        itemsToCompleteLevel.text = _oreToComleteLevelText + oreToCompleteLevel;
    }
}