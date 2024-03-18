using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //Behaviour for inventory slots. Populates and depopulates

    [HideInInspector]
    public ItemData item = new ItemData();
    public ItemInspection inspector;
    
    public void OnClick()
    {
        if (item.itemDescription != null)
        {
            GameObject.Find("Item Description").GetComponent<Text>().text = item.itemDescription;
            inspector.OnInspect(item, true);
        }
    }

    public void OnExit() {
        GameObject.Find("Item Description").GetComponent<Text>().text = "";
    }
}
