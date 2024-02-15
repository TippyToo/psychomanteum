using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[Tooltip("Dialogue settings for NPC")]
public struct Conversation
{
    [TextArea(5, 50)]
    [Tooltip("List of each dialogue box in the current \"conversation\"")]
    public string[] sentences;
    [Tooltip("List of each dialogue boxes image in the current \"conversation\" (Leave blank for no change)")]
    public Sprite[] dialogueBoxImage;
    [Tooltip("Speed that each sentence box is spoken (Bigger = faster. If not listed for a sentence, element 0 will be the default)")]
    public float[] talkSpeed;
    [Tooltip("List of each npc portrait for each sentence (If not listed for a sentence, element 0 will be the default)")]
    public Sprite[] portrait;
    [Tooltip("Enter the element number for the next conversation chunk to load (Leave blank or set to -1 if there is no more dialogue unlocked. If Has Dialogue Tree is checked, enter multiple numbers corresponding to each response option even if they all go to the same place)")]
    public int[] nextDialogue;
    [Tooltip("If checked, when the current conversation element ends, prompts user for a response")]
    public bool hasDialogueTree;
    [Tooltip("Only populate if Has Dialogue Tree is checked (Max of three options/up to element 2)")]
    public string[] playerResponses;
    [Tooltip("If checked, starts the next conversation right away (Only functional with Has Dialogue Tree enabled)")]
    public bool persistentConversation;
}
