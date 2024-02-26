using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour, IDataPersistance, ISettings
{
    [Tooltip("ONLY FILL OUT IF DATA SHOULD BE SAVED")]
    public string npcName;

    //Defaults
    private const float DEFAULT_TALK_SPEED = 1.0f;
    public Sprite DEFAULT_DIALOGUE_BOX_IMAGE;
    public Sprite DEFAULT_NPC_SPEAKER_SPRITE;

    
    private bool detectsPlayer;

    //To avoid confusion, isTalking is for detecting if the npc has begun talking to show the text box
    //speaking is used for detecting if the dialogue box is currently writing out what the npc is saying
    private bool isTalking;
    private bool speaking;
    private bool responding = false;

    private PlayerController player;
    private AudioSource audSource;
    public AudioClip[] talkSound;
    private float talkVolume;

    public int maxDialogueLength;

    private GameObject dialogueBox;
    private GameObject playerResponseBox;
    private Button[] playerResponses;
    

    private Text dialogueText;
    private string currentFullText;

    public Conversation[] conversation;
    private Queue<string> currentDialogue;
    private int conversationToLoad = 0;
    private int currSentence = 0;
    private SpriteRenderer NPCImage;
    private Image dialogueBoxImage;

    private JournalManager journal;


    //Indicates end of current dialogue 
    private GameObject arrow;

    public void SaveData(ref SaveData data) {
        if (data != null) 
        {
            //data.conversationToLoad = conversationToLoad;
            if (data.currentConversation.ContainsKey(npcName))
            {
                data.currentConversation.Remove(npcName);
            }
            data.currentConversation.Add(npcName, conversationToLoad);
        }
        else 
            Debug.Log("No Save Slot Found"); 
    }
    public void LoadData(SaveData data) { 
        //this.conversationToLoad = data.conversationToLoad;
        
        data.currentConversation.TryGetValue(npcName, out conversationToLoad);
    }


    // Start is called before the first frame update
    void Start()
    {
        currentDialogue = new Queue<string>();
        playerResponseBox = GameObject.Find("UI").transform.GetChild(2).gameObject;
        playerResponses = new Button[]{ playerResponseBox.transform.GetChild(0).GetChild(0).GetComponent<Button>(),
        playerResponseBox.transform.GetChild(0).GetChild(1).GetComponent<Button>(), playerResponseBox.transform.GetChild(0).GetChild(2).GetComponent<Button>() };
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        dialogueBoxImage = dialogueBox.transform.GetChild(0).GetComponent<Image>();
        NPCImage = dialogueBox.transform.GetChild(1).GetComponent<SpriteRenderer>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
        detectsPlayer = false;
        isTalking = false;
        speaking = false;
        audSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        journal = GameObject.Find("Journal").GetComponent<JournalManager>();
        ApplySettings();
    }

    // Update is called once per frame
    void Update()
    {
        InteractButton();
        if ((speaking || isTalking || responding) && journal.IsPaused())
        {
            dialogueBox.transform.GetChild(0).gameObject.SetActive(false);
            dialogueBox.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if ((speaking || isTalking || responding) && !journal.IsPaused()) 
        {
            dialogueBox.transform.GetChild(0).gameObject.SetActive(true);
            dialogueBox.transform.GetChild(1).gameObject.SetActive(true);
        }
        //Locks the players movement while talking
        if (detectsPlayer) { player.talking = isTalking; }
    }


    //Determine how the interact button should function based on the current state of dialogue
    private void InteractButton() {
        if (detectsPlayer && Input.GetButtonUp("Interact") && !responding && !journal.isOpen)
        {
            if (!isTalking)
            {
                //If not talking and next dialogue is unlocked (cTL != 0), start a dialogue. If not, "stare" at the player in silence
                if (conversationToLoad != -1 && conversationToLoad >= 0)
                { CreateDialogue(conversation[conversationToLoad]); }
                else
                { CreateDialogue(". . ."); }
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
                arrow.SetActive(false);
                if (currentDialogue.Count() <= 0)
                { EndConversation(); }
                else
                {
                    currSentence++;
                    StartCoroutine(WriteText());
                }
            }
        }
    }


    //Seperates each sentence box in the current conversation and queues them up to be written out
    private void CreateDialogue(Conversation conversation) {
        
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


    //Overload to take a single string instead of an entire conversation (needed to be stared at)
    private void CreateDialogue(string nothing) {
        currentDialogue.Clear();
        currentDialogue.Enqueue(nothing);
        dialogueBox.SetActive(true);
        StartCoroutine(NothingText());
    }

    //Same as WriteText but functions for the (. . .) case
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

    //Writes out the queued sentence character by character and removes it from the queue
    private IEnumerator WriteText() {

        isTalking = true;
        speaking = true;
        currentFullText = currentDialogue.Dequeue();
        string currText;
        float talkSpeed;

        //Set Dialogue Speed
        if (currSentence > (conversation[conversationToLoad].talkSpeed.Count() - 1))
        { talkSpeed = DEFAULT_TALK_SPEED; }
        else if (currSentence > conversation[conversationToLoad].talkSpeed.Count())
        { talkSpeed = conversation[conversationToLoad].talkSpeed[0]; }
        else
        { talkSpeed = conversation[conversationToLoad].talkSpeed[currSentence]; }

        //Set Dialogue Box's Image
        if (currSentence > (conversation[conversationToLoad].dialogueBoxImage.Count() - 1))
        { dialogueBoxImage.sprite = DEFAULT_DIALOGUE_BOX_IMAGE; }
        else if (currSentence > conversation[conversationToLoad].dialogueBoxImage.Count())
        { dialogueBoxImage.sprite = conversation[conversationToLoad].dialogueBoxImage[0]; }
        else
        { dialogueBoxImage.sprite = conversation[conversationToLoad].dialogueBoxImage[currSentence]; }

        //Set NPC Dialogue Image
        if (currSentence > (conversation[conversationToLoad].portrait.Count() - 1))
        { NPCImage.sprite = DEFAULT_NPC_SPEAKER_SPRITE; }
        else if (currSentence > conversation[conversationToLoad].portrait.Count())
        { NPCImage.sprite = conversation[conversationToLoad].portrait[0]; }
        else
        { NPCImage.sprite = conversation[conversationToLoad].portrait[currSentence]; }

        //Writes out the text character by character with selected settings
        for (int i = 1; i < currentFullText.Length + 1; i++)
        {
            
            int sound = Random.Range(0, talkSound.Length);
            currText = currentFullText.Substring(0, i);
            if (!currText.EndsWith(" ")) { audSource.PlayOneShot(talkSound[sound], talkVolume); }
            dialogueText.text = currText;

            if (talkSpeed == DEFAULT_TALK_SPEED)
            { talkSpeed *= PlayerPrefs.GetInt("Text Speed"); }

            yield return new WaitForSeconds(1 / (talkSpeed * 5));
        }
        speaking = false;
        StartCoroutine(ArrowBlink());
    }

    //After the NPC finishes their dialogue, either prompts the user with responses or closes the dialgue
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
    }

    //If there are player responses, populate buttons with response text, and target if applicable
    private void CreatePlayerResponses() {
        
        playerResponseBox.SetActive(true);
        for (int i = 0; i < conversation[conversationToLoad].playerResponses.Count(); i++) {
            if (conversation[conversationToLoad].nextDialogue.Count() <= 0)
            {
                playerResponses[i].GetComponent<ResponseButton>().toLoad = -1;
            }
            else { 
                playerResponses[i].GetComponent<ResponseButton>().toLoad = conversation[conversationToLoad].nextDialogue[i]; }
            playerResponses[i].GetComponent<ResponseButton>().NPC = this.gameObject;
            playerResponses[i].interactable = true;
            playerResponses[i].transform.GetChild(0).GetComponent<Text>().text = conversation[conversationToLoad].playerResponses[i];
        }
    }

    //After the player chooses their response, reset the buttons and prep the next conversation if applicable
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

    //Makes the next sentence arrow blink 
    private IEnumerator ArrowBlink() {
        while (true)
        {
            arrow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

    //Colliders
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = true; player = other.gameObject.GetComponent<PlayerController>(); }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { detectsPlayer = false; }
    }

    //Settings
    public void ApplySettings()
    {
        talkVolume = PlayerPrefs.GetFloat("Dialogue");
    }
}
