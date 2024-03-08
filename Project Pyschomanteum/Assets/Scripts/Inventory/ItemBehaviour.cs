using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour, IDataPersistance
{
    public string itemName;
    public string itemDescription;
    [Tooltip("Write what chapter the item appears in. Used for determining where in the journal the itwm will appear.")]
    public int chapter;
    public Sprite itemImage;
    public bool collected = false;
    private ItemData itemData;

    private bool detectsPlayer = false;
    private void Awake()
    {
        itemData = new ItemData(itemName, itemDescription, chapter, collected);
    }
    public void SaveData(ref SaveData data)
    {
        
    }

    public void LoadData(SaveData data)
    {
        data.collectedItems.TryGetValue(itemName, out collected);
        if (collected) { gameObject.SetActive(false); }
    }


    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact") && !collected)
        {
            AddToInventory();
        }
    }

    public void AddToInventory()
    {
        //Adds items to inventory and marks it as collected
        itemData.collect();
        collected = true;
        GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().totalInventory.Add(this.itemData);
        GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().UpdateInventory();
        gameObject.SetActive(false);
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
