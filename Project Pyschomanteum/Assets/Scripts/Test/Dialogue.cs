using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    //Handles creating dialogue boxes, text and audio. Speaker images to be added

    private bool detectsPlayer;

    //To avoid confusion, isTalking is for detecting if the npc has begun talking to show the text box
    //speaking is used for detecting if the dialogue box is currently writing out what the npc is saying
    private bool isTalking;
    private bool speaking;

    private PlayerController player;
    private AudioSource audSource;
    public AudioClip[] talkSound;

    public int maxDialogueLength;

    private GameObject dialogueBox;
    private GameObject playerResponseBox;
    private Button[] playerResponses;
    private bool responding = false;
    private Text dialogueText;
    private string currentFullText;

    public Conversation[] conversation;
    private Queue<string> currentDialogue;
    private int conversationToLoad = 0;
    private int currSentence = 0;
    private SpriteRenderer dialogueImage;


    //Indicates end of current dialogue 
    private GameObject arrow;



    // Start is called before the first frame update
    void Start()
    {
        currentDialogue = new Queue<string>();
        playerResponseBox = GameObject.Find("UI").transform.GetChild(2).gameObject;
        playerResponses = new Button[]{ playerResponseBox.transform.GetChild(0).GetChild(0).GetComponent<Button>(),
        playerResponseBox.transform.GetChild(0).GetChild(1).GetComponent<Button>(), playerResponseBox.transform.GetChild(0).GetChild(2).GetComponent<Button>() };
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        dialogueImage = dialogueBox.transform.GetChild(1).GetComponent<SpriteRenderer>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
        detectsPlayer = false;
        isTalking = false;
        speaking = false;
        audSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact") && !responding)
        {
            //Determine how the interact button should function based on the dialogue
            if (!isTalking)
            {
                //If not talking, start a dialogue
                if (conversationToLoad != -1 && conversationToLoad >= 0)
                {
                    CreateDialogue(conversation[conversationToLoad]);
                    //CreateDialogue(dialogue[dialogueOption], talkDelay);
                }
                else { CreateDialogue(". . ."); }
            }
            else if (speaking)
            {
                //If currently speaking, end it early and display all dialogue without waiting
                StopAllCoroutines();
                speaking = false;
                dialogueText.text = currentFullText;
                StartCoroutine(ArrowBlink());
            }
            else
            {
                StopAllCoroutines();
                //Go to the next piece of dialogue, or end the dialogue if nothing is left
                if (currentDialogue.Count() <= 0)//currNum + 1 == currentTotalText.Count)
                {
                    EndConversation();
                }
                else
                {
                    currSentence++;
                    StartCoroutine(WriteText());
                }
            }
        }
        //Locks the players movement while talking
        if (detectsPlayer) { player.canMove = !isTalking; }
    }

    private void CreateDialogue(Conversation conversation)//string dialogue, float talkDelay)
    {
        currentDialogue.Clear();
        for (int i = 0; i < conversation.sentences.Count(); i++)
        {
            if (conversation.sentences[i].Length > maxDialogueLength) {
                for (int x = 0; conversation.sentences[i].Length > maxDialogueLength; x++)
                {
                    currentDialogue.Enqueue(conversation.sentences[i].Substring(0, maxDialogueLength));
                    conversation.sentences[i] = conversation.sentences[i].Substring(maxDialogueLength, conversation.sentences[i].Length - maxDialogueLength);
                }
                if (conversation.sentences[i].Length <= maxDialogueLength && conversation.sentences[i].Length > 0)
                { 
                    currentDialogue.Enqueue(conversation.sentences[i]); 
                }
            } else { 
                currentDialogue.Enqueue(conversation.sentences[i]); 
            }
        }
        dialogueBox.SetActive(true);
        StartCoroutine(WriteText());
    }
    private void CreateDialogue(string nothing) {
        currentDialogue.Clear();
        currentDialogue.Enqueue(nothing);
        dialogueBox.SetActive(true);
        StartCoroutine(NothingText());
    }
    private IEnumerator NothingText() {
        isTalking = true;
        speaking = true;
        currentFullText = currentDialogue.Dequeue();
        string currText;
        for (int i = 0; i < currentFullText.Length + 1; i++)
        {
            currText = currentFullText.Substring(0, i);
            dialogueText.text = currText;
            yield return new WaitForSeconds(0.5f);
        }
        speaking = false;
        StartCoroutine(ArrowBlink());
    }
    private IEnumerator WriteText()//tring fullText, float talkDelay)
    {
        //Writes out the dialogue character by character
        isTalking = true;
        dialogueImage.sprite = conversation[conversationToLoad].portrait[currSentence];
        speaking = true;
        currentFullText = currentDialogue.Dequeue();
        string currText;
        for (int i = 0; i < currentFullText.Length + 1; i++)
        {
            int sound = Random.Range(0, talkSound.Length);
            currText = currentFullText.Substring(0, i);
            if (!currText.EndsWith(" ")) { audSource.PlayOneShot(talkSound[sound], 1.0f); }
            dialogueText.text = currText;
            yield return new WaitForSeconds(conversation[conversationToLoad].talkSpeed[currSentence]);
        }
        speaking = false;
        StartCoroutine(ArrowBlink());
    }
    private void EndConversation() {
        currSentence = 0;
        dialogueBox.SetActive(false);
        if (conversationToLoad == -1) {
            isTalking = false;
            return;
        }
        if (conversation[conversationToLoad].hasDialogueTree)
        {
            responding = true;
            CreatePlayerResponses();
        }
        else if (conversation[conversationToLoad].nextDialogue.Count() > 0) {
            conversationToLoad = conversation[conversationToLoad].nextDialogue[0];
            isTalking = false;
        } else  { conversationToLoad = -1; isTalking = false; }
        Debug.Log(conversationToLoad);
    }
    private void CreatePlayerResponses() {
        playerResponseBox.SetActive(true);
        for (int i = 0; i < conversation[conversationToLoad].playerResponses.Count(); i++) {
            if (conversation[conversationToLoad].nextDialogue.Count() <= 0)
            {
                playerResponses[i].GetComponent<ResponseButton>().toLoad = -1;
            }
            else { playerResponses[i].GetComponent<ResponseButton>().toLoad = conversation[conversationToLoad].nextDialogue[i]; }
            playerResponses[i].GetComponent<ResponseButton>().NPC = this.gameObject;
            playerResponses[i].interactable = true;
            playerResponses[i].transform.GetChild(0).GetComponent<Text>().text = conversation[conversationToLoad].playerResponses[i];
        }
    }
    public void PlayersResponse(int toLoad) {
        for (int i = 0; i < conversation[conversationToLoad].playerResponses.Count(); i++)
        {
            playerResponses[i].interactable = false;
            playerResponses[i].transform.GetChild(0).GetComponent<Text>().text = "";
            playerResponses[i].onClick.RemoveAllListeners();
        }

        if (conversation[conversationToLoad].persistentConversation)
        {
            conversationToLoad = toLoad;
            CreateDialogue(conversation[conversationToLoad]);
        }
        else
        {
            conversationToLoad = toLoad;
            isTalking = false;
        }
        playerResponseBox.SetActive(false);
        responding = false;
    }

    private IEnumerator ArrowBlink()
    {
        while (true)
        {
            arrow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = true; player = other.gameObject.GetComponent<PlayerController>(); }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = false; }
    }
}
