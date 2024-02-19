using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Add things that would need to be saved here. Including but is not limited to journal entries, dialogue, checkpoints, inventory, world falgs, etc.
    public int conversationToLoad;

    public SaveData() { 
        this.conversationToLoad = 0;
    }
}
