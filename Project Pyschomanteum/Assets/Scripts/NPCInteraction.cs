using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public bool detectsPlayer;

    //To avoid confusion, isTalking is for detecting if the npc has begun talking to show the text box
    //speaking is used for detecting if the dialogue box is currently writing out what the npc is saying
    public bool isTalking;
    public bool speaking;
    public float talkSpeed;

    private string[] currentTotalText;
    private string currentFullText;
    private int currNum = 0;

    //public Sprite[] dialogueSprites; 
    public GameObject dialogueBox;
    private Text dialogueText;

    //Indicates end of current dialogue 
    public GameObject arrow;

    //Determine Interactions
    public bool firstInteract = true;
    public bool secondInteract = true;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
        detectsPlayer = false;
        isTalking = false;
        speaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonDown("Interact")) 
        { 
            Debug.Log("Input Successful");
            if (!isTalking) { 
                //If not talking, start a dialogue
                if (firstInteract) { CreateDialogue(new string[] {"Boo!"}, talkSpeed); firstInteract = false; }
                else if (secondInteract) { CreateDialogue(new string[] { "Did I scare you?", "Sorry." }, talkSpeed); secondInteract = false; }
            } else if (speaking) {
                //If currently speaking, end it early and display all dialogue without waiting
                StopCoroutine("WriteText");
                dialogueText.text = currentFullText;
                StartCoroutine(ArrowBlink());
            } else {
                StopCoroutine("ArrowBlink");
                //Go to the next piece of dialogue, or end the dialogue if nothing is left
                if (currNum == currentTotalText.Length) {
                    dialogueBox.SetActive(false);
                    currNum = 0;
                } else {
                    currNum += 1;
                    StartCoroutine(WriteText(currentTotalText[currNum], talkSpeed));
                }
            }
        }
    }
    
    public void CreateDialogue(string[] dialogue, float talkSpeed) {
        //Opens a dialogue box
        isTalking = true;
        speaking = true;
        currentTotalText = dialogue;
        dialogueBox.SetActive(true);
        StartCoroutine(WriteText(dialogue[0], talkSpeed));
    }

    private IEnumerator WriteText(string fullText, float talkSpeed) {
        //Writes out the dialogue character by character, complete with a speed scaler
        currentFullText = fullText;
        string currText;
        for (int i = 0; i < fullText.Length; i++) {
            currText = fullText.Substring(0, i);
            dialogueText.text = currText;
            yield return new WaitForSeconds(talkSpeed);
        }
        speaking = false;
        StartCoroutine(ArrowBlink());
    }

    private IEnumerator ArrowBlink() {
        while (true) { 
            arrow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") { detectsPlayer = true; }
    }
    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") { detectsPlayer = false; }
    }
}
