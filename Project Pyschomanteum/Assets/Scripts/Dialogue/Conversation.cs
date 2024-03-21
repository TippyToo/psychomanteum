using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[Tooltip("Dialogue settings for NPC")]
public struct Conversation
{
    [Tooltip("List of each NPC dialogue box in the current \"conversation\" (Can be left blank if player will speak)")]
    public Sentence[] npcSentences;
    [Tooltip("List of each player dialogue box in the current \"conversation\" (Only spoken after ALL npc sentences for the current element are finished (If there are none then they will be spoken right away). To have a back and forth, check persistent conversation)")]
    public Sentence[] playerSentences;
    [Tooltip("If checked, starts the next conversation right away")]
    public bool persistentConversation;
    [Tooltip("Enter the element number for the next conversation chunk to load (Leave blank or set to -1 if there is no more dialogue unlocked. If Has Dialogue Tree is checked, enter multiple numbers corresponding to each response option even if they all go to the same place)")]
    public int[] nextDialogue;
    [Tooltip("If checked, when the current conversation element ends, prompts user for a response")]
    public bool hasDialogueTree;
    [Tooltip("Only populate if Has Dialogue Tree is checked (Max of three options/up to element 2)")]
    public string[] playerResponses;
}
