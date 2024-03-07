using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string itemName;
    public string itemDescription;
    [Tooltip("Write wgat chapter the item appears in. Used for determining where in the journal the itwm will appear.")]
    public int chapter;
    public Sprite itemImage;
    public bool collected;

    public void collect() { collected = true; }
    public ItemData(string name, string description, int chapterFound, bool isCollected) { 
        itemName = name;
        itemDescription = description;
        chapter = chapterFound;
        collected = isCollected;
    }
    public ItemData() {
        itemName = null;
        itemDescription = null;
        chapter = 0;
        collected = false;
    }
    

    

    
    
}
