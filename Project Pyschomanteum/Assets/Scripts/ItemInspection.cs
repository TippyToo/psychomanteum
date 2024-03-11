using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInspection : MonoBehaviour, IDragHandler
{
    private InventoryManager inventoryManager;
    private GameObject itemPrefab;
    private void Awake()
    {
        inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
    }

    public void OnInspect(ItemData item) {
        if (itemPrefab != null) {
            Destroy(itemPrefab.gameObject);
        }
        itemPrefab = Instantiate(Resources.Load(item.itemName), new Vector3(10000, 10000, 10000), Quaternion.identity, GameObject.Find("ItemToInspect").transform) as GameObject;
    }
    public void OnDrag(PointerEventData eventData) {
        itemPrefab.transform.eulerAngles += new Vector3(-eventData.delta.y, -eventData.delta.x);
    }
    public void DeleteInspectedObject() {
        if (itemPrefab != null) { 
            Destroy(itemPrefab.gameObject); 
        }
    }

    public void DisableChildren() {
        foreach (Transform child in transform) { 
            child.gameObject.SetActive(false);
        }
    }
}
