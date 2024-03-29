﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    private void OnTriggerEnter(Collider other)
    {
        ManagersProvider.Inventory.AddItem(itemName);
        Destroy(this.gameObject);
        if (itemName == "Ore")
        {
            Messenger.Broadcast(GameEvent.ORE_UPDATED);
        }
    }
}