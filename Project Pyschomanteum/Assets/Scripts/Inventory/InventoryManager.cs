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
    public GameObject inventorySlots;
    private Text currentInventoryText;

    public List<VerbalClueData> totalClues = new List<VerbalClueData>();
    public GameObject clueSlots;
    private Text currentClueText;

    [HideInInspector]
    public int currentInventory = 0;

    void Awake()
    {
        
    }
    
    public void LoadData(SaveData data) {
        //Objects
        foreach (var value in data.collectedItemDescriptions) { 
            ItemData temp = new ItemData();
            temp.itemName = value.Key;
            temp.itemDescription = value.Value;
            temp.collected = true;
            data.collectedItemChapters.TryGetValue(temp.itemName, out temp.chapter);
            totalInventory.Add(temp);
        }

        //Clues
        foreach (var value in data.clueDescriptions) { 
            VerbalClueData temp = new VerbalClueData();
            temp.name = value.Key;
            temp.description = value.Value;
            data.clueChapters.TryGetValue(temp.name, out temp.chapter);
            data.clueIssuers.TryGetValue(temp.name, out temp.issuer);
            totalClues.Add(temp);
        }
    }

    public void SaveData(ref SaveData data) {
        if (data != null)
        {
            //Objects
            if (totalInventory.Count != 0) {
                foreach (var i in totalInventory) {
                    if (data.collectedItemDescriptions.ContainsKey(i.itemName)) { data.collectedItemDescriptions.Remove(i.itemName); }
                    if (data.collectedItemChapters.ContainsKey(i.itemName)) { data.collectedItemChapters.Remove(i.itemName); }
                    if (data.collectedItems.ContainsKey(i.itemName)) { data.collectedItems.Remove(i.itemName); }
                    data.collectedItemDescriptions.Add(i.itemName, i.itemDescription);
                    data.collectedItemChapters.Add(i.itemName, i.chapter);
                    data.collectedItems.Add(i.itemName, i.collected);
                }
            }

            //Clues
            if (totalClues.Count != 0) {
                foreach (var i in totalClues) {
                    if (data.clueIssuers.ContainsKey(i.name)) { data.clueIssuers.Remove(i.name); }
                    if (data.clueDescriptions.ContainsKey(i.name)) { data.clueDescriptions.Remove(i.name); }
                    if (data.clueChapters.ContainsKey(i.name)) { data.clueChapters.Remove(i.name); }
                    data.clueIssuers.Add(i.name, i.issuer);
                    data.clueDescriptions.Add(i.name, i.description);
                    data.clueChapters.Add(i.name, i.chapter);
                }
            }
        }
        else
            Debug.Log("No Save Slot Found");
    }

    // Start is called before the first frame update
    void Start()
    {
        currentInventoryText = inventorySlots.transform.GetChild(0).GetComponent<Text>();
        currentClueText = clueSlots.transform.GetChild(0).GetComponent<Text>();
        UpdateInventory(GameObject.Find("Level Manager").GetComponent<LevelManager>().level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInventory() {
        //Updates the inventory page to display the currently selected inventory items
        currentInventoryText.text = "Inventory\nChapter " + currentInventory;
        currentClueText.text = "Clues\nChapter " + currentInventory;
        //Set all slot descriptions to be empty
        for (int i = 1; i < 6; i++)
        {
            inventorySlots.transform.GetChild(i).GetComponent<InventorySlot>().item = new ItemData();
            clueSlots.transform.GetChild(i).GetComponent<ClueSlot>().clue = new VerbalClueData();
        }

        //Populate each slot with an item name and description based on how many items have been collected
        int x = 1;
        int y = 1;
        foreach (ItemData i in totalInventory)
        {
            if (i.chapter == currentInventory)
            {
                Transform invSlot = inventorySlots.transform.GetChild(x);
                invSlot.gameObject.SetActive(true);
                invSlot.GetComponent<Text>().text = i.itemName;
                invSlot.GetComponent<InventorySlot>().item = i;
                x++;
            }
        }
        foreach (VerbalClueData i in totalClues) {
            if (i.chapter == currentInventory) { 
                Transform clueSlot = clueSlots.transform.GetChild(y);
                clueSlot.gameObject.SetActive(true);
                clueSlot.GetComponent<Text>().text = i.name;
                clueSlot.GetComponent<ClueSlot>().clue = i;
                y++;
            }
        }
        //Deactivate remaining slots
        for (; x < inventorySlots.transform.childCount; x++)
        {
            inventorySlots.transform.GetChild(x).gameObject.SetActive(false);
        }
        for (; y < clueSlots.transform.childCount; y++)
        {
            clueSlots.transform.GetChild(y).gameObject.SetActive(false);
        }

    }

    public void UpdateInventory(int level)
    {
        //Overload that also updates the inventory page but to display a specified inventory
        if (level != currentInventory) {
            GameObject temp = GameObject.Find("ItemToInspect");
            if (temp.transform.childCount > 0) {
                foreach (Transform child in temp.transform) { 
                    Destroy(child.gameObject);
                }
                GameObject.Find("Item Description").GetComponent<Text>().text = "";
            }
            clueSlots.transform.parent.GetChild(clueSlots.transform.parent.childCount - 1).GetComponent<Text>().text = "";
        }
        currentInventory = level;
        currentInventoryText.text = "Inventory\nChapter " + currentInventory;
        currentClueText.text = "Clues\nChapter " + currentInventory;

        //Set all slot descriptions to be empty
        for (int i = 1; i < 6; i++)
        {
            inventorySlots.transform.GetChild(i).GetComponent<InventorySlot>().item = new ItemData();
            clueSlots.transform.GetChild(i).GetComponent<ClueSlot>().clue = new VerbalClueData();
        }

        //Populate each slot with an item name and description based on how many items have been collected
        int x = 1;
        int y = 1;
        foreach (ItemData i in totalInventory) {
            if (i.chapter == level) {
                Transform invSlot = inventorySlots.transform.GetChild(x);
                invSlot.gameObject.SetActive(true);
                invSlot.GetComponent<Text>().text = i.itemName;
                invSlot.GetComponent<InventorySlot>().item = i;
                x++;
            }
        }
        foreach (VerbalClueData i in totalClues)
        {
            if (i.chapter == level)
            {
                Transform clueSlot = clueSlots.transform.GetChild(y);
                clueSlot.gameObject.SetActive(true);
                clueSlot.GetComponent<Text>().text = i.name;
                clueSlot.GetComponent<ClueSlot>().clue = i;
                y++;
            }
        }
        //Deactivate remaining slots
        for (; x < inventorySlots.transform.childCount; x++) {
            inventorySlots.transform.GetChild(x).gameObject.SetActive(false);
        }
        for (; y < clueSlots.transform.childCount; y++)
        {
            clueSlots.transform.GetChild(y).gameObject.SetActive(false);
        }

    }
    public void AddClue(VerbalClueData data) { 
        totalClues.Add(data);
        UpdateInventory();
        StartCoroutine(Scribble());
    }
    private IEnumerator Scribble() { 
    
        yield return null;
    }
}
