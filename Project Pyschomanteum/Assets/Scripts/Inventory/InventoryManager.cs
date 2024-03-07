using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour, IDataPersistance
{
    //public static InventoryManager Instance { get; private set; }
    public List<ItemData> inventory = new List<ItemData>();
    public List<string> inventoryText2;
    public Text inventoryText;


    void Awake()
    {
        //if (Instance != null) { Debug.Log("More than one inventory manager. Destroying new one."); Destroy(this.gameObject); return; }
        //Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    public void LoadData(SaveData data) {
        foreach (var value in data.collectedItemDescriptions) { 
            ItemData temp = new ItemData();
            temp.itemName = value.Key;
            temp.itemDescription = value.Value;
            temp.collected = true;
            //data.collectedItems.TryGetValue(temp.itemName, out temp.collected);
            inventory.Add(temp);
        }
    }
    public void SaveData(ref SaveData data) {
        if (data != null)
        {
            if (inventory.Count != 0)
            {
                foreach (var i in inventory)
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
        UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateInventory() {
        List<string> names = new List<string>();
        if (inventory != null)
        {
            foreach (var i in inventory)
            {
                names.Add(i.itemName);
            }
        }
        names.Sort();
        inventoryText.text = "";
        foreach (var i in names) {
            inventoryText.text += i + '\n';
        }
    }
}
