using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IDataPersistance
{
    private string itemName;
    public string itemDescription;
    [Tooltip("Write what chapter the item appears in. Used for determining where in the journal the itwm will appear.")]
    public int chapter;
    public Sprite itemImage;
    public bool collected = false;
    private ItemData itemData;
    private ItemInspection itemInspector;

    private bool detectsPlayer = false;
    private void Awake()
    {
        itemName = transform.name;
        itemInspector = GameObject.Find("Item Inspection").GetComponent<ItemInspection>();
        itemData = new ItemData(itemName, itemDescription, chapter, collected);
        if (transform.parent != null && transform.parent.name != "ItemToInspect") { GetComponent<Collider>().enabled = true; } 
        else if (transform.parent == null) { GetComponent<Collider>().enabled = true; }
    }
    public void SaveData(ref SaveData data)
    {
        
    }

    public void LoadData(SaveData data)
    {
        if (data != null)
        {
            data.collectedItems.TryGetValue(itemName, out collected);
            if (collected) { gameObject.SetActive(false); }
        }
    }


    void Update()
    {
        if (transform.parent != null && transform.parent.name == "ItemToInspect") {
            GetComponent<Collider>().enabled = false;
        }
        if (detectsPlayer && Input.GetButtonUp("Interact") && !collected)
        {
            GameObject.Find("UI").transform.GetChild(0).GetComponent<JournalManager>().canOpen = false;
            itemInspector.OnInspect(itemData);
            GameObject.Find("Player").GetComponent<PlayerController>().DisableMovement();
            AddToInventory();
        }
    }

    public void AddToInventory(bool fromNPC = false)
    {
        //Adds items to inventory and marks it as collected
        itemData.collect();
        GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().totalInventory.Add(this.itemData);
        GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().UpdateInventory();
        if (fromNPC == false)
        {
            gameObject.SetActive(false);
            collected = true;
        }
    }

    public void AddToInventoryFromDialogue() {
        itemName = transform.name;
        itemInspector = GameObject.Find("Item Inspection").GetComponent<ItemInspection>();
        itemData = new ItemData(itemName, itemDescription, chapter, collected);
        GameObject.Find("UI").transform.GetChild(0).GetComponent<JournalManager>().canOpen = false;
        itemInspector.OnInspect(itemData);
        GameObject.Find("Player").GetComponent<PlayerController>().DisableMovement();
        AddToInventory(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = true; }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = false; }
    }
}
