using UnityEngine;

[System.Serializable]
public struct EndBehaviour
{
    [Tooltip("Check to make the player present the correct clue to progress. Correct guesses load Next Dialogue element0. Wrong guesses load Next Dialogue element1")]
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
    [Tooltip("Name of the scene to change to after dialogue ends.")]
    public string changeScene;
    [Tooltip("After completing the current conversation, set the next one to trigger automatically when in range")]
    public bool proxyTalk;
    [Tooltip("After completing the current conversation, follow the player")]
    public bool followPlayerAfter;
}
