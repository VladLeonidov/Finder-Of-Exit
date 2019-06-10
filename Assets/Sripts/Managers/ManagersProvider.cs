using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(WeatherManager))]
[RequireComponent(typeof(ImagesManager))]
[RequireComponent(typeof(AudioManager))]
[RequireComponent(typeof(MissionManager))]
[RequireComponent(typeof(DataManager))]
public class ManagersProvider : MonoBehaviour
{
    public static PlayerManager Player { get; private set; }
    public static InventoryManager Inventory { get; private set; }
    public static WeatherManager Weather { get; private set; }
    public static ImagesManager Images { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static MissionManager Mission { get; private set; }
    public static DataManager Data { get; private set; }

    private List<IGameManager> _startSequence;

    private static ManagersProvider instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (!ReferenceEquals(instance, this))
        {
            return;
        }

        Data = GetComponent<DataManager>();
        Player = GetComponent<PlayerManager>();
        Inventory = GetComponent<InventoryManager>();
        //Weather = GetComponent<WeatherManager>();
        Images = GetComponent<ImagesManager>();
        Audio = GetComponent<AudioManager>();
        Mission = GetComponent<MissionManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Inventory);
        //_startSequence.Add(Weather);
        _startSequence.Add(Images);
        _startSequence.Add(Audio);
        _startSequence.Add(Mission);
        _startSequence.Add(Data);

        StartupManagers();
    }

    private void StartupManagers()
    {
        NetworkService network = new NetworkService();

        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup(network);
        }

        int numModules = _startSequence.Count;
        int numModulesReady = 0;
        int numModulesShutdown = 0;

        while (numModulesReady < numModules)
        {
            numModulesReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Started)
                {
                    numModulesReady++;
                }
            }

            Debug.Log("Progress: " + numModulesReady + "/" + numModules);
            Messenger<int, int>.Broadcast
                (StartupEvent.MANAGERS_PROGRESS, numModulesReady, numModules);

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.Status == ManagerStatus.Shutdown)
                {
                    numModulesShutdown++;
                }
            }

            if (numModulesShutdown > 0)
            {
                Debug.LogError("Some manaagers dropped!");
                return;
            }
        }

        Debug.Log("All managers started up!");
        Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
    }
}