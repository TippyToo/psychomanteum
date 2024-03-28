using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour, IDataPersistance, ISettings
{
    [Tooltip("ONLY FILL OUT IF DATA SHOULD BE SAVED")]
    public string npcName;
    public bool speakOnProximity;
    //Defaults
    public float DEFAULT_TALK_SPEED = 1.0f;
    public Sprite DEFAULT_DIALOGUE_BOX_IMAGE;
    public Sprite DEFAULT_NPC_SPEAKER_SPRITE;
    public Sprite DEFAULT_PLAYER_SPEAKER_SPRITE;
    public int maxDialogueLength;
    public Conversation[] conversation;
    public AudioClip[] talkSound;


    private bool detectsPlayer = false;

    //To avoid confusion, isTalking is for detecting if the npc has begun talking to show the text box
    //speaking is used for detecting if the dialogue box is currently writing out what the npc is saying
    private bool isTalking = false;
    private bool speaking = false;
    private bool responding = false;

    [HideInInspector]
    public PlayerController player;
    private AudioSource audSource;
    public AudioClip clueFound;

    private float talkVolume;

    private GameObject dialogueBox;
    private GameObject playerResponseBox;
    private Button[] playerResponses;
    

    private Text dialogueText;
    private string currentFullText;
    private Queue<string> currentDialogue = new Queue<string>();
    private int conversationToLoad = 0;
    private int currSentence = 0;

    private List<float> dialogueTalkSpeeds;
    private List<Sprite> dialogueBoxImages;
    private List<int> dialogueClues;
    private List<int> objectClues;


    private SpriteRenderer NPCImage;
    private Image dialogueBoxImage;
    private JournalManager journal;

    private IsInteractable interact;


    //Indicates end of current dialogue 
    private GameObject arrow;

    #region Save and Load
    public void SaveData(ref SaveData data) {
        if (data != null) 
        {
            if (data.currentConversation.ContainsKey(npcName))
            {
                data.currentConversation.Remove(npcName);
            }
            data.currentConversation.Add(npcName, conversationToLoad);
        }
        else 
            Debug.Log("No Save Slot Found"); 
    }
    public void LoadData(SaveData data) { if (data != null) data.currentConversation.TryGetValue(npcName, out conversationToLoad); }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerResponseBox = GameObject.Find("UI").transform.GetChild(2).gameObject;
        playerResponses = new Button[]{ playerResponseBox.transform.GetChild(0).GetChild(0).GetComponent<Button>(),
        playerResponseBox.transform.GetChild(0).GetChild(1).GetComponent<Button>(), playerResponseBox.transform.GetChild(0).GetChild(2).GetComponent<Button>() };
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        dialogueBoxImage = dialogueBox.transform.GetChild(0).GetComponent<Image>();
        NPCImage = dialogueBox.transform.GetChild(1).GetComponent<SpriteRenderer>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
        audSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        journal = GameObject.Find("Journal").GetComponent<JournalManager>();
        interact = transform.GetChild(0).GetComponent<IsInteractable>();
        ApplySettings();
    }

    // Update is called once per frame
    void Update()
    {
        detectsPlayer = interact.detectsPlayer;
        InteractButton();
        if (Input.GetButtonUp("Journal"))
        {
            if ((speaking || isTalking || responding) && journal.IsPaused())
            {
                dialogueBox.SetActive(false);
            }
            else if ((speaking || isTalking || responding) && !journal.IsPaused())
            {
                dialogueBox.SetActive(true);
            }
        }
        //Locks the players movement while talking
        if (detectsPlayer) { player.canMove = !isTalking; }
    }


    //Determine how the interact button should function based on the current state of dialogue
    private void InteractButton() {
        if (detectsPlayer && Input.GetButtonUp("Interact") && !responding && !journal.isOpen && !speakOnProximity)
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
                dialogueBox.transform.GetChild(2).gameObject.SetActive(false);
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
        
        currentDialogue.Clear(); //Clear queue

        dialogueTalkSpeeds = new List<float>();
        dialogueBoxImages = new List<Sprite>();
        dialogueClues = new List<int>();
        objectClues = new List<int>();


        //Check and add NPC dialogue for the current conversation
        if (conversation.npcSentences != null && conversation.npcSentences.Count() > 0)
        {
            for (int i = 0; i < conversation.npcSentences.Count(); i++)
            {
                string sentenceText = conversation.npcSentences[i].sentenceText;
                if (sentenceText.Length > maxDialogueLength)
                {
                    for (int x = 0; sentenceText.Length > maxDialogueLength; x++)
                    {
                        currentDialogue.Enqueue(sentenceText.Substring(0, maxDialogueLength));
                        sentenceText = sentenceText.Substring(maxDialogueLength, sentenceText.Length - maxDialogueLength);
                    }
                    if (sentenceText.Length <= maxDialogueLength && sentenceText.Length > 0)
                    {
                        currentDialogue.Enqueue(sentenceText);
                    }
                }
                else { currentDialogue.Enqueue(sentenceText); }

                if (conversation.npcSentences[i].talkSpeed != 0.0f) { dialogueTalkSpeeds.Add(conversation.npcSentences[i].talkSpeed); }
                else { dialogueTalkSpeeds.Add(DEFAULT_TALK_SPEED); }

                if (conversation.npcSentences[i].dialogueBoxImage != null) { dialogueBoxImages.Add(conversation.npcSentences[i].dialogueBoxImage); }
                else { dialogueBoxImages.Add(DEFAULT_DIALOGUE_BOX_IMAGE); }

                if (conversation.npcSentences[i].isVerbalClue) { dialogueClues.Add(1); }
                else { dialogueClues.Add(0); }

                if (conversation.npcSentences[i].objectClue) { objectClues.Add(1); }
                else { objectClues.Add(0); }
            }
        }
        //Check and add player dialogue for the current conversation
        if (conversation.playerSentences != null && conversation.playerSentences.Count() > 0) {
            for (int i = 0; i < conversation.playerSentences.Count(); i++)
            {
                string sentenceText = conversation.playerSentences[i].sentenceText;
                if (sentenceText.Length > maxDialogueLength)
                {
                    for (int x = 0; sentenceText.Length > maxDialogueLength; x++)
                    {
                        currentDialogue.Enqueue(sentenceText.Substring(0, maxDialogueLength));
                        sentenceText = sentenceText.Substring(maxDialogueLength, sentenceText.Length - maxDialogueLength);
                    }
                    if (sentenceText.Length <= maxDialogueLength && sentenceText.Length > 0)
                    {
                        currentDialogue.Enqueue(sentenceText);
                    }
                }
                else { currentDialogue.Enqueue(sentenceText); }

                if (conversation.playerSentences[i].talkSpeed != 0.0f) { dialogueTalkSpeeds.Add(conversation.playerSentences[i].talkSpeed); }
                else { dialogueTalkSpeeds.Add(DEFAULT_TALK_SPEED); }

                if (conversation.playerSentences[i].dialogueBoxImage != null) { dialogueBoxImages.Add(conversation.playerSentences[i].dialogueBoxImage); }
                else { dialogueBoxImages.Add(DEFAULT_DIALOGUE_BOX_IMAGE); }

                if (conversation.playerSentences[i].isVerbalClue) { dialogueClues.Add(1); }
                else { dialogueClues.Add(0); }

                if (conversation.playerSentences[i].objectClue) { objectClues.Add(1); }
                else { objectClues.Add(0); }
            }
        }
        
        dialogueBox.SetActive(true);
        dialogueBox.transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(WriteText());
    }


    //Overload to take a single string instead of an entire conversation (needed to be stared at)
    private void CreateDialogue(string nothing) {
        currentDialogue.Clear();
        currentDialogue.Enqueue(nothing);
        dialogueBox.SetActive(true);
        dialogueBox.transform.GetChild(1).gameObject.SetActive(true);
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
        float talkSpeed = dialogueTalkSpeeds[currSentence];
        dialogueBoxImage.sprite = dialogueBoxImages[currSentence];
        

        //Set Speaker Portraits
        if ((currSentence + 1) > conversation[conversationToLoad].npcSentences.Count()) 
        {
            //Set Player Portraits
            if (conversation[conversationToLoad].playerSentences.Count() > 0 && conversation[conversationToLoad].playerSentences[currSentence - conversation[conversationToLoad].npcSentences.Count()].speakerPortrait != null) 
            { playerResponseBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = conversation[conversationToLoad].playerSentences[currSentence - conversation[conversationToLoad].npcSentences.Count()].speakerPortrait; }
            else 
            { playerResponseBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = DEFAULT_PLAYER_SPEAKER_SPRITE; }
            dialogueBox.transform.GetChild(1).gameObject.SetActive(false);
            playerResponseBox.transform.GetChild(1).gameObject.SetActive(true);

        }
        else 
        { 
            // Set NPC Portraits
            if (conversation[conversationToLoad].npcSentences.Count() > 0 && conversation[conversationToLoad].npcSentences[currSentence].speakerPortrait != null)
            { NPCImage.sprite = conversation[conversationToLoad].npcSentences[currSentence].speakerPortrait; }
            else
            { NPCImage.sprite = DEFAULT_NPC_SPEAKER_SPRITE; }
            dialogueBox.transform.GetChild(1).gameObject.SetActive(true);
            playerResponseBox.transform.GetChild(1).gameObject.SetActive(false);

        }

        //Writes out the text character by character with selected settings
        for (int i = 1; i < currentFullText.Length + 1; i++)
        {
            int sound = Random.Range(0, talkSound.Length);
            currText = currentFullText.Substring(0, i);
            if (!currText.EndsWith(" ")) { audSource.PlayOneShot(talkSound[sound], 1); }
            dialogueText.text = currText;

            if (talkSpeed == DEFAULT_TALK_SPEED)
            { talkSpeed *= PlayerPrefs.GetInt("Text Speed"); }

            yield return new WaitForSeconds(1 / (talkSpeed * 5));
        }

        speaking = false;
        StartCoroutine(ArrowBlink());
    }

    #region End Behaviour
    //Makes the next sentence arrow blink 
    private IEnumerator ArrowBlink()
    {
        CheckForClues();
        while (true)
        {
            arrow.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
    #region Clues
    private void CheckForClues()
    {
        //Check for clues
        if (conversationToLoad != -1)
        {
            if (dialogueClues[currSentence] == 1)
            {
                //Create new container and add it to journal
                VerbalClueData clue;
                if ((currSentence + 1) > conversation[conversationToLoad].npcSentences.Count())
                    clue = new VerbalClueData(conversation[conversationToLoad].playerSentences[currSentence - conversation[conversationToLoad].npcSentences.Count()].clueName, this.npcName, conversation[conversationToLoad].playerSentences[currSentence - conversation[conversationToLoad].npcSentences.Count()].clueText, GameObject.Find("Level Manager").GetComponent<LevelManager>().level);
                else
                    clue = new VerbalClueData(conversation[conversationToLoad].npcSentences[currSentence].clueName, this.npcName, conversation[conversationToLoad].npcSentences[currSentence].clueText, GameObject.Find("Level Manager").GetComponent<LevelManager>().level);
                GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().AddClue(clue);
                StartCoroutine(Scribble());
            }
            if (objectClues[currSentence] == 1)
            {
                conversation[conversationToLoad].npcSentences[currSentence].objectClue.GetComponent<ItemBehaviour>().AddToInventoryFromDialogue();
                StartCoroutine(Scribble());
            }
        }
    }
    private IEnumerator Scribble()
    {
        Debug.Log("Do");
        audSource.PlayOneShot(clueFound, 1);
        dialogueBox.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        dialogueBox.transform.GetChild(2).gameObject.SetActive(false);
    }
    #endregion

    //After the NPC finishes their dialogue, cleans up and prepares the end behaviour
    private void EndConversation() {
        currSentence = 0;
        dialogueBox.SetActive(false);
        playerResponseBox.transform.GetChild(1).gameObject.SetActive(false);
        if (conversationToLoad == -1) {
            isTalking = false;
            return;
        }

        EndBehaviour endBehaviour = conversation[conversationToLoad].endBehaviour;

        if (endBehaviour.presentClues) {
            PresentClues();
        }

        if (endBehaviour.warp) {
            if (endBehaviour.hasDialogueTree) { Debug.LogError("Can not warp and respond to an NPC at the same time", transform); }
            if (!endBehaviour.presentClues)
            { player.transform.position = endBehaviour.warpLocation; }
            else 
            {
                PresentClues(true);
            }
        }

        //Determines what conversation to load next
        if (endBehaviour.hasDialogueTree)
        {
            responding = true;
            CreatePlayerResponses();
        } else if (!endBehaviour.warp && conversation[conversationToLoad].persistentConversation && conversation[conversationToLoad].nextDialogue[0] != -1) {
            conversationToLoad = conversation[conversationToLoad].nextDialogue[0];
            CreateDialogue(conversation[conversationToLoad]);
        }
        else if (conversation[conversationToLoad].nextDialogue.Count() > 0) {
            conversationToLoad = conversation[conversationToLoad].nextDialogue[0];
            isTalking = false;
        } else { conversationToLoad = -1; isTalking = false; }

        if (endBehaviour.setNextConversationToStartOnProximity) {
            speakOnProximity = true;
        }
    }

    private void PresentClues(bool warpAfter = false) { 
    
    }

    //If there are player responses, populate buttons with response text, and target if applicable
    private void CreatePlayerResponses() {
        playerResponseBox.transform.GetChild(0).gameObject.SetActive(true);
        playerResponseBox.transform.GetChild(1).gameObject.SetActive(true);
        playerResponseBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = DEFAULT_PLAYER_SPEAKER_SPRITE;
        for (int i = 0; i < conversation[conversationToLoad].endBehaviour.playerResponses.Count(); i++) {
            if (conversation[conversationToLoad].nextDialogue.Count() <= 0)
            {
                playerResponses[i].GetComponent<ResponseButton>().toLoad = -1;
            }
            else 
            { 
                playerResponses[i].GetComponent<ResponseButton>().toLoad = conversation[conversationToLoad].nextDialogue[i]; 
            }
            playerResponses[i].GetComponent<ResponseButton>().NPC = this.gameObject;
            playerResponses[i].interactable = true;
            playerResponses[i].transform.GetChild(0).GetComponent<Text>().text = conversation[conversationToLoad].endBehaviour.playerResponses[i];
        }
    }

    //After the player chooses their response, reset the buttons and prep the next conversation if applicable
    public void PlayersResponse(int toLoad) {
        
        for (int i = 0; i < conversation[conversationToLoad].endBehaviour.playerResponses.Count(); i++)
        {
            playerResponses[i].interactable = false;
            playerResponses[i].transform.GetChild(0).GetComponent<Text>().text = "";
            playerResponses[i].onClick.RemoveAllListeners();
        }

        if (conversation[conversationToLoad].persistentConversation)
        {
            conversationToLoad = toLoad;
            if (conversationToLoad >= 0)
                CreateDialogue(conversation[conversationToLoad]);
            else {
                playerResponseBox.transform.GetChild(1).gameObject.SetActive(false);
                isTalking = false;
            }
        }
        else
        {
            conversationToLoad = toLoad;
            isTalking = false;
            playerResponseBox.transform.GetChild(1).gameObject.SetActive(false);
        }
        playerResponseBox.transform.GetChild(0).gameObject.SetActive(false);
        responding = false;
    }
    #endregion

    public void SpeakOnProximity() {
        if (speakOnProximity)
        {
            //If not talking and next dialogue is unlocked (cTL != 0), start a dialogue. If not, "stare" at the player in silence
            if (conversationToLoad != -1 && conversationToLoad >= 0)
            { CreateDialogue(conversation[conversationToLoad]); }
            else
            { CreateDialogue(". . ."); }
            speakOnProximity = false;
        }
    }
    //Settings
    public void ApplySettings()
    {
        talkVolume = PlayerPrefs.GetFloat("Dialogue");
    }
}
