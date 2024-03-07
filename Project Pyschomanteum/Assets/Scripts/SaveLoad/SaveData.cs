using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int chapter;

    //Add things that would need to be saved here. Including but is not limited to journal entries, dialogue, checkpoints, inventory, world falgs, etc.
    //Clues
    public SerializableDictionary<string, bool> collectedItems;
    public SerializableDictionary<string, int> collectedItemChapters;
    public SerializableDictionary<string, string> collectedItemDescriptions;
    //public SerializableDictionary<string, Sprite> collectedItemImages;

    public SerializableDictionary<string, int> currentConversation;
    public Vector3 playerPosition;

    public SaveData() {
        chapter = 1;
        collectedItems = new SerializableDictionary<string, bool>();
        collectedItemChapters = new SerializableDictionary<string, int>();
        collectedItemDescriptions = new SerializableDictionary<string, string>();
        //collectedItemImages = new SerializableDictionary<string, Sprite>();

        currentConversation = new SerializableDictionary<string, int>();
    }
}
