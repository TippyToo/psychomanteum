using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[Tooltip("Settings for each individual text box")]
public struct Sentence
{
    [TextArea(5, 50)]
    [Tooltip("Text of the dialogue box")]
    public string sentenceText;
    [Tooltip("Image for the dialogue box (Leave blank for default)")]
    public Sprite dialogueBoxImage;
    [Tooltip("Portrait for the dialogue box (Leave blank for default)")]
    public Sprite speakerPortrait;
    [Tooltip("Speed the sentence is spoken (Bigger = faster. Leave blank for default)")]
    public float talkSpeed;
    [Tooltip("If filled, when the sentence ends the object will be added to the journal")]
    public GameObject objectClue;
    [Tooltip("If checked, when the sentence ends a verbal clue will be added to the journal")]
    public bool isVerbalClue;
    [Tooltip("name of the clue")]
    public string clueName;
    [TextArea(5, 50)]
    [Tooltip("What the clue will say in the journal")]
    public string clueText;
}
