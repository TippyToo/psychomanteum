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
    [Tooltip("If checked, when the sentence ends a verbal clue will be added to the journal")]
    public bool isClue;
    [Tooltip("name of the clue")]
    public string clueName;
    [Tooltip("What the clue will say in the journal")]
    public string clueText;
}
