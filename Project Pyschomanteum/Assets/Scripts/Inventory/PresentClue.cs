using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PresentClue : MonoBehaviour
{
    public ItemData itemToPresent = null;
    public VerbalClueData clueData = null;

    public JournalManager journalManager;

    public string correctItem = null;

    public int correctLoad;
    public int wrongLoad;

    public string toPresent;

    public Dialogue NPC;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick() {
        GameObject temp = journalManager.transform.GetChild(7).gameObject;
        if (itemToPresent != null) {
            journalManager.PushSection(temp);
            journalManager.canClose = false;
            toPresent = itemToPresent.itemName;
            temp.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to present the clue " + toPresent + "?";
        }
        else if (clueData != null) {
            journalManager.PushSection(temp);
            journalManager.canClose = false;
            toPresent = clueData.name;
            temp.transform.GetChild(0).GetComponent<Text>().text = "Are you sure you want to present the clue " + toPresent + "?";
        }
    }
    public void Yes() {
        journalManager.PopSection();
        journalManager.canClose = true;

        //Check if it's the right clue. NPC responds accordingly
        if (toPresent != null) {
            if (toPresent == correctItem)
            {
                Debug.Log("Correct");
                NPC.conversationToLoad = correctLoad;
            }
            else {
                Debug.Log("Wrong");
                NPC.conversationToLoad = wrongLoad;
            }
        }
        journalManager.CloseJournal();
        NPC.CreateDialogue(NPC.conversation[NPC.conversationToLoad]);

    }

    public void No() { journalManager.PopSection(); }
    public void OnClose() {
        itemToPresent = null;
        clueData = null;
        toPresent = null;
        correctItem = null;
        journalManager.isPresenting = false;
        gameObject.SetActive(false);
    }
}
