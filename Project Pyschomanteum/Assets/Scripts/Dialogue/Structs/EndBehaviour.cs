using UnityEngine;

[System.Serializable]
public struct EndBehaviour
{
    [Tooltip("Not implemented yet")]
    public bool presentClues;
    [Tooltip("Name of the clue that will advance the dialogue")]
    public string correctClueName;
    [Tooltip("Check to warp to the coordinates after the conversation ends. Overrides persistant conversation")]
    public bool warp;
    [Tooltip("X Y Z coordinates of where to warp")]
    public Vector3 warpLocation;
    [Tooltip("If checked, when the current conversation element ends, prompts user for a response")]
    public bool hasDialogueTree;
    [Tooltip("Only populate if Has Dialogue Tree is checked (Max of three options/up to element 2)")]
    public string[] playerResponses;
    public bool setNextConversationToStartOnProximity;
}
