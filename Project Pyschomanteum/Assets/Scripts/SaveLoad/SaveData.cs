using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Add things that would need to be saved here. Including but is not limited to journal entries, dialogue, checkpoints, inventory, world falgs, etc.

    public int chapter;
    public string sceneToLoad; 

    //Objects
    public SerializableDictionary<string, bool> collectedItems;
    public SerializableDictionary<string, int> collectedItemChapters;
    public SerializableDictionary<string, string> collectedItemDescriptions;

    //Clues
    public SerializableDictionary<string, string> clueIssuers;
    public SerializableDictionary<string, string> clueDescriptions;
    public SerializableDictionary<string, int> clueChapters;


    public SerializableDictionary<string, int> currentConversation;
    public Vector3 playerPosition;

    public SaveData() {
        chapter = 1;
        sceneToLoad = "Iona's Pyschomanteum";
        collectedItems = new SerializableDictionary<string, bool>();
        collectedItemChapters = new SerializableDictionary<string, int>();
        collectedItemDescriptions = new SerializableDictionary<string, string>();

        clueIssuers = new SerializableDictionary<string, string>();
        clueDescriptions = new SerializableDictionary<string, string>();
        clueChapters = new SerializableDictionary<string, int>();

        currentConversation = new SerializableDictionary<string, int>();
    }
}
