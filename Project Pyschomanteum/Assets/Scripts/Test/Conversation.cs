using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[Tooltip("Dialogue settings for NPC")]
public class Conversation
{
    [TextArea(5, 50)]
    [Tooltip("List of each dialogue box in the current \"conversation\"")]
    public string[] sentences;
    [Tooltip("Speed that each sentence box is spoken")]
    public float[] talkSpeed;
    [Tooltip("List of each npc portrait for each sentence (Smaller = Faster)")]
    public Sprite[] portrait;
    [Tooltip("Enter the element number for the next conversation chunk to load (Leave blank if there is no more dialogue unlocked. If Has Dialogue Tree is checked, enter multiple numbers corresponding to each response option even if they all go to the same place)")]
    public int[] nextDialogue;
    [Tooltip("If checked, when the current conversation element ends, prompts user for a response")]
    public bool hasDialogueTree;
    [Tooltip("If checked, starts the next conversation right away (Only functional with Has Dialogue Tree enabled)")]
    public bool persistentConversation;
    [Tooltip("Only populate if Has Dialogue Tree is checked (Max of three options/up to element 2)")]
    public string[] playerResponses;
}
