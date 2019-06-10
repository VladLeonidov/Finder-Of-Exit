using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField]
    private int countItemsToCompleteMission;

    private void OnTriggerEnter(Collider other)
    {
        if (ManagersProvider.Inventory.GetItemCount("Ore") >= countItemsToCompleteMission)
        {
            ManagersProvider.Mission.ReachObjective();
        }
    }
}