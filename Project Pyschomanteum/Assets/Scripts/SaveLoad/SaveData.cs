using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int chapter;
    public string saveImagePath;
    //public Sprite saveImage => Resources.Load<Sprite>("Sprites/carrots");

    //Add things that would need to be saved here. Including but is not limited to journal entries, dialogue, checkpoints, inventory, world falgs, etc.
    public int conversationToLoad;

    public SaveData() {
        this.chapter = 1;
        saveImagePath = "Sprites/carrots";
        this.conversationToLoad = 0;
    }
}
