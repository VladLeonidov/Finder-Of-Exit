using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    private NetworkService _network;

    public ManagerStatus Status { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public bool IsDiePlayer { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager starting...");

        _network = service;

        UpdateData(50, 100);

        IsDiePlayer = false;

        Status = ManagerStatus.Started;
    }

    public void UpdateData(int currentHealth, int maxHealth)
    {
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
    }

    public void ChangeHealth(int value)
    {
        CurrentHealth += value;
        if (CurrentHealth >= MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            IsDiePlayer = true;
        }

        if (CurrentHealth == 0)
        {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        Messenger.Broadcast(GameEvent.HEALTH_UPDATED);
    }

    public void PlayerRespawn()
    {
        UpdateData(50, 100);
        IsDiePlayer = false;
    }
}