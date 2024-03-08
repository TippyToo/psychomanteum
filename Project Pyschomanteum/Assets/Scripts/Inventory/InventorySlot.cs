using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //Behaviour for inventory slots. Populates and depopulates

    [HideInInspector]
    public string description = "";
    
    public void OnHover()
    {
        if (description != "")
            GameObject.Find("Item Description").GetComponent<Text>().text = description;
    }

    public void OnExit() {
        GameObject.Find("Item Description").GetComponent<Text>().text = "";
    }
}
