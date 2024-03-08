using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static Cinemachine.DocumentationSortingAttribute;

public class InventoryManager : MonoBehaviour, IDataPersistance
{
    public List<ItemData> totalInventory = new List<ItemData>();
    private List<ItemData> displayedInventory = new List<ItemData>();
    public GameObject inventorySlots;
    [HideInInspector]
    public int currentInventory = 0;


    void Awake()
    {
        
    }

    public void LoadData(SaveData data) {
        foreach (var value in data.collectedItemDescriptions) { 
            ItemData temp = new ItemData();
            temp.itemName = value.Key;
            temp.itemDescription = value.Value;
            temp.collected = true;
            data.collectedItemChapters.TryGetValue(temp.itemName, out temp.chapter);
            totalInventory.Add(temp);
        }
    }

    public void SaveData(ref SaveData data) {
        if (data != null)
        {
            if (totalInventory.Count != 0)
            {
                foreach (var i in totalInventory)
                {
                    if (data.collectedItemDescriptions.ContainsKey(i.itemName))
                    {
                        data.collectedItemDescriptions.Remove(i.itemName);
                    }
                    if (data.collectedItemChapters.ContainsKey(i.itemName))
                    {
                        data.collectedItemChapters.Remove(i.itemName);
                    }
                    if (data.collectedItems.ContainsKey(i.itemName))
                    {
                        data.collectedItems.Remove(i.itemName);
                    }
                    data.collectedItemDescriptions.Add(i.itemName, i.itemDescription);
                    data.collectedItemChapters.Add(i.itemName, i.chapter);
                    data.collectedItems.Add(i.itemName, i.collected);
                }
            }
        }
        else
            Debug.Log("No Save Slot Found");
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateInventory(GameObject.Find("Level Manager").GetComponent<LevelManager>().level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInventory() {
        //Updates the inventory page to display the currently selected inventory items
        displayedInventory = new List<ItemData>();

        //Set all slot descriptions to be empty
        for (int i = 1; i < 6; i++)
        {
            inventorySlots.transform.GetChild(i).GetComponent<InventorySlot>().description = "";
        }

        //Populate each slot with an item name and description based on how many items have been collected
        int x = 1;
        foreach (ItemData i in totalInventory)
        {
            if (i.chapter == currentInventory)
            {
                displayedInventory.Add(i);
                Transform invSlot = inventorySlots.transform.GetChild(x);
                invSlot.gameObject.SetActive(true);
                invSlot.GetComponent<Text>().text = i.itemName;
                invSlot.GetComponent<InventorySlot>().description = i.itemDescription;
                x++;
            }
        }

        //Deactivate remaining slots
        for (; x < inventorySlots.transform.childCount; x++)
        {
            inventorySlots.transform.GetChild(x).gameObject.SetActive(false);
        }

    }
    public void UpdateInventory(int level)
    {
        //Overload that also updates the inventory page but to display a specified inventory
        currentInventory = level;
        displayedInventory = new List<ItemData>();

        //Set all slot descriptions to be empty
        for (int i = 1; i < 6; i++) {
            inventorySlots.transform.GetChild(i).GetComponent<InventorySlot>().description = "";
        }

        //Populate each slot with an item name and description based on how many items have been collected
        int x = 1;
        foreach (ItemData i in totalInventory) {
            if (i.chapter == level) {
                displayedInventory.Add(i);
                Transform invSlot = inventorySlots.transform.GetChild(x);
                invSlot.gameObject.SetActive(true);
                invSlot.GetComponent<Text>().text = i.itemName;
                invSlot.GetComponent<InventorySlot>().description = i.itemDescription;
               x++;
            }
        }

        //Deactivate remaining slots
        for (; x < inventorySlots.transform.childCount; x++) {
            inventorySlots.transform.GetChild(x).gameObject.SetActive(false);
        }

    }
}
