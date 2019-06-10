using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour, IGameManager
{
    private string _fileName;
    private NetworkService _network;

    public ManagerStatus Status { get; private set; }

    public void Startup(NetworkService service = null)
    {
        Debug.Log("Data manager starting...");
        _network = service;
        _fileName = Path.Combine(Application.persistentDataPath, "game.dat");

        Status = ManagerStatus.Started;
    }

    public void SaveGameState()
    {
        Dictionary<string, object> gameState = new Dictionary<string, object>();
        gameState.Add("inventory", ManagersProvider.Inventory.GetData());
        gameState.Add("currentHealth", ManagersProvider.Player.CurrentHealth);
        gameState.Add("maxHeath", ManagersProvider.Player.MaxHealth);
        gameState.Add("currentLevel", ManagersProvider.Mission.CurrentLevel);
        gameState.Add("maxLevel", ManagersProvider.Mission.MaxLevel);

        SerealizeGameState(gameState);

        Debug.Log("Game saved!");
    }

    public void LoadGameState()
    {
        if (!File.Exists(_fileName))
        {
            Debug.Log("No saved game");
            return;
        }

        Dictionary<string, object> gameState = DeserealizeGameState();

        ManagersProvider.Inventory.
            UpdateData((Dictionary<string, int>) gameState["inventory"]);
        ManagersProvider.Player.
            UpdateData((int) gameState["currentHealth"], (int) gameState["maxHeath"]);
        ManagersProvider.Mission.
            UpdateData((int)gameState["currentLevel"], (int)gameState["maxLevel"]);
        ManagersProvider.Mission.RestartCurrent(false);

        Debug.Log("Game loaded!");
    }

    private void SerealizeGameState(Dictionary<string, object> gameState)
    {
        using (FileStream stream = File.Create(_fileName))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, gameState);
            stream.Close();
        }
    }

    private Dictionary<string, object> DeserealizeGameState()
    {
        Dictionary<string, object> result;

        using (FileStream stream = File.Open(_fileName, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            result = formatter.Deserialize(stream) as Dictionary<string, object>;
            stream.Close();
        }

        return result;
    }
}