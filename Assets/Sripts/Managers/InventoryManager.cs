using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private Dictionary<string, int> _items;
    private NetworkService _network;

    public ManagerStatus Status { get; private set; }

    public string EquippedItem { get; private set; }

    public void Startup(NetworkService service)
    {
        Debug.Log("Invertory manager starting...");
        _network = service;
        UpdateData(new Dictionary<string, int>());
        Status = ManagerStatus.Started;
    }

    public void ResetInventory()
    {
        if (_items != null)
        {
            _items.Clear();
            EquippedItem = null;
        }
    }

    public void AddItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name] += 1;
        }
        else
        {
            _items[name] = 1;
        }

        DispleyItems();
    }

    public List<string> GetItemList()
    {
        return new List<string>(_items.Keys);
    }

    public int GetItemCount(string itemName)
    {
        if (_items.ContainsKey(itemName))
        {
            return _items[itemName];
        }

        return 0;
    }

    public bool EquipItem(string name)
    {
        if (_items.ContainsKey(name) && EquippedItem != name)
        {
            EquippedItem = name;
            Debug.Log("Equipped " + name);
            return true;
        }

        EquippedItem = null;
        Debug.Log("Unequipped");
        return false;
    }

    public bool ConsumeItem(string name)
    {
        if (_items.ContainsKey(name))
        {
            _items[name]--;
            if (_items[name] == 0)
            {
                _items.Remove(name);
            }
        }
        else
        {
            Debug.Log("Cannot consume " + name);
            return false;
        }

        DispleyItems();
        return true;
    }

    public void UpdateData(Dictionary<string, int> items)
    {
        _items = items;
    }

    public Dictionary<string, int> GetData()
    {
        return _items;
    }

    private void DispleyItems()
    {
        StringBuilder itemsDisplay = new StringBuilder("Items: ");
        foreach (KeyValuePair<string, int> item in _items)
        {
            itemsDisplay.Append(item.Key);
            itemsDisplay.Append("(");
            itemsDisplay.Append(item.Value);
            itemsDisplay.Append(") ");
        }

        Debug.Log(itemsDisplay.ToString());
    }
}