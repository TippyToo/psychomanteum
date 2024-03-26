using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChapterTab : MonoBehaviour
{
    private GameObject inventory;
    public int inventoryLoad = 0;

    private void Awake()
    {
        if (inventory == null) inventory = GameObject.Find("Inventory Manager");
    }

    public void OnClick() {
        inventory.transform.GetComponent<InventoryManager>().UpdateInventory(inventoryLoad);
    }
}
