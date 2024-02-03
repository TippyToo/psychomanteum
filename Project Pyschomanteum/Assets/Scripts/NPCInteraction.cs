using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    private bool detectsPlayer;

    //To avoid confusion, isTalking is for detecting if the npc has begun talking to show the text box
    //speaking is used for detecting if the dialogue box is currently writing out what the npc is saying
    private bool isTalking;
    private bool speaking;

    //Determines the speed the NPC talks. Smaller = faster
    public float talkDelay;

    private PlayerController player;
    private AudioSource audSource;
    public AudioClip[] talkSound;

    public int maxDialogueLength;
    [TextArea]
    public string[] dialogue;
    private int dialogueOption = 0;

    private List<string> dialogueBoxes;
    public List<int> dialogueLength;
    //Stores the current text blurb and dialogue string
    private List<string> currentTotalText;
    private string currentFullText;
    private int currNum = 1;

    //public Sprite[] dialogueSprites; 
    private GameObject dialogueBox;
    private Text dialogueText;

    //Indicates end of current dialogue 
    private GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
        detectsPlayer = false;
        isTalking = false;
        speaking = false;
        audSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) 
        { 
            //Determine how the interact button should function based on the dialogue
            if (!isTalking) { 
                //If not talking, start a dialogue
                if (dialogueOption < dialogue.Length)
                CreateDialogue(dialogue[dialogueOption], talkDelay);
            } else if (speaking) {
                //If currently speaking, end it early and display all dialogue without waiting
                StopAllCoroutines();
                speaking = false;
                dialogueText.text = currentFullText;
                StartCoroutine(ArrowBlink());
            } else {
                StopAllCoroutines();
                //Go to the next piece of dialogue, or end the dialogue if nothing is left
                if (currNum == currentTotalText.Count) {
                    dialogueBox.SetActive(false);
                    isTalking = false;
                    currNum = 1;
                    dialogueOption += 1;
                } else {
                    StartCoroutine(WriteText(currentTotalText[currNum], talkDelay));
                    currNum += 1;
                }
            }
        }
        //Locks the players movement while talking
        if (detectsPlayer) { player.canMove = !isTalking; }
    }
    
    private void CreateDialogue(string dialogue, float talkDelay) {
        //Opens a dialogue box
        dialogueBoxes = new List<string>();
        isTalking = true;

        //Determines where to seperate the dialogue boxes by detecting either the max length one can be or where it has been spacified with the return key
        dialogueLength = new List<int>();
        for (int i = 0; i < dialogue.Length;) {
            if ((dialogue.IndexOf("\n", i) != -1) && dialogue.IndexOf("\n", i) < (i + maxDialogueLength)) {
                dialogueLength.Add((dialogue.IndexOf("\n") + 1));
                i += dialogue.IndexOf("\n") + 1;
            } else {
                i += maxDialogueLength;
                if (i < dialogue.Length) { 
                    dialogueLength.Add(maxDialogueLength);
                }
            }
        }
        //Cut up dialogue into seperate boxes here
        for (int i = 0; dialogueLength.Count > i; i++) { 
            dialogueBoxes.Add(dialogue.Substring(0, dialogueLength[i]));
            dialogue = dialogue.Substring(dialogueLength[i], dialogue.Length - dialogueLength[i]);
        }
        if (dialogue.Length <= maxDialogueLength && dialogue.Length > 0) { 
            dialogueBoxes.Add(dialogue);
        }
        currentTotalText = dialogueBoxes;
        dialogueBox.SetActive(true);
        StartCoroutine(WriteText(currentTotalText[0], talkDelay));
    }

    private IEnumerator WriteText(string fullText, float talkDelay) {
        //Writes out the dialogue character by character
        speaking = true;
        currentFullText = fullText;
        string currText;
        for (int i = 0; i < fullText.Length + 1; i++) {
            int sound = Random.Range(0, talkSound.Length);
            currText = fullText.Substring(0, i);
            if (!currText.EndsWith(" ")) { audSource.PlayOneShot(talkSound[sound], 1.0f); }
            dialogueText.text = currText;
            yield return new WaitForSeconds(talkDelay);
        }
        speaking = false;
        StartCoroutine(ArrowBlink());
    }

    private IEnumerator ArrowBlink() {
        //Makes the arrow blink
        while (true) { 
            arrow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") { detectsPlayer = true; player = other.gameObject.GetComponent<PlayerController>(); }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") { detectsPlayer = false; }
    }
}
