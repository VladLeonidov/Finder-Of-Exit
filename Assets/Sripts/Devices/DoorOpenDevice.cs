using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenDevice : BaseDevice
{
    [SerializeField]
    private Vector3 dPos;
    [SerializeField]
    private bool needKey;

    private bool _open;

	public override void Operate()
    {
        if (needKey)
        {
            if (ManagersProvider.Inventory.EquippedItem == "Key")
            {
                OpenCloseDoor();
            }
        }
        else
        {
            OpenCloseDoor();
        }
    }

    private void OpenCloseDoor()
    {
        if (_open)
        {
            Vector3 pos = this.transform.position - this.dPos;
            this.transform.position = pos;
        }
        else
        {
            Vector3 pos = this.transform.position + dPos;
            this.transform.position = pos;
        }

        _open = !_open;
    }
}