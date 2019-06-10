using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField]
    private Image[] itemIcons;
    [SerializeField]
    private Text[] itemLabels;

    [SerializeField]
    private Text currentItemLabel;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Button useButton;

    private string _currentItem;

    public void Refresh()
    {
        List<string> itemsList = ManagersProvider.Inventory.GetItemList();

        int len = itemIcons.Length;
        for (int i = 0; i < len; i++)
        {
            if (i < itemsList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                string item = itemsList[i];

                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize();

                int count = ManagersProvider.Inventory.GetItemCount(item);
                string message = "x" + count;
                if (item == ManagersProvider.Inventory.EquippedItem)
                {
                    message = "Equipped\n" + message;
                }
                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) =>
                {
                    OnItem(item);
                });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemsList.Contains(_currentItem))
        {
            _currentItem = null;
        }

        if (_currentItem == null)
        {
            currentItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            currentItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (_currentItem == "Health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }

            currentItemLabel.text = _currentItem + ":";
        }
    }

    public void OnItem(string item)
    {
        _currentItem = item;
        Refresh();
    }

    public void OnEquip()
    {
        ManagersProvider.Inventory.EquipItem(_currentItem);
        Refresh();
    }

    public void OnUse()
    {
        ManagersProvider.Inventory.ConsumeItem(_currentItem);
        if (_currentItem == "Health")
        {
            ManagersProvider.Player.ChangeHealth(25);
        }
        Refresh();
    }
}