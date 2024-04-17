using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Observation : MonoBehaviour
{
    public float DEFAULT_TALK_SPEED = 1.0f;
    public Sprite DEFAULT_DIALOGUE_BOX_IMAGE;
    public Sprite DEFAULT_PLAYER_SPEAKER_SPRITE;
    public Sentence[] observations;

    public AudioClip[] talkSound;
    public AudioClip clueFound;

    private bool isTalking = false;
    private bool speaking = false;

    private GameObject dialogueBox;
    private TextMeshProUGUI dialogueText;
    private GameObject playerResponseBox;
    private GameObject arrow;

    private string currentFullText;
    private int currSentence = 0;

    private float talkVolume;
    private bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        playerResponseBox = GameObject.Find("UI").transform.GetChild(2).gameObject;
        dialogueBox = GameObject.Find("UI").transform.GetChild(1).gameObject;
        dialogueText = dialogueBox.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        arrow = dialogueBox.transform.GetChild(0).transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        InteractButton();
    }
    private void InteractButton()
    {
        if (clicked)
        {
            if (Input.GetButtonUp("Interact"))
            {
                if (speaking)
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
                    if (currSentence >= observations.Length - 1 && isTalking)
                    { EndConversation(); }
                    else
                    {
                        currSentence++;
                        StartCoroutine(WriteText());
                    }
                }
            }
        }
    }

    private IEnumerator WriteText()
    {
        isTalking = true;
        speaking = true;
        currentFullText = observations[currSentence].sentenceText;
        string fullText = currentFullText;
        string currText;
        float talkSpeed;
        
        dialogueBox.SetActive(true);
        dialogueBox.transform.GetChild(1).gameObject.SetActive(false);
        playerResponseBox.transform.GetChild(1).gameObject.SetActive(true);

        //Set talk speed
        if (observations[currSentence].talkSpeed != 0) { talkSpeed = observations[currSentence].talkSpeed; }
        else {talkSpeed = DEFAULT_TALK_SPEED; }

        talkSpeed *= PlayerPrefs.GetInt("Text Speed");

        //Set dialogue box image
        if (observations[currSentence].dialogueBoxImage != null) { dialogueBox.transform.GetChild(0).GetComponent<Image>().sprite = observations[currSentence].dialogueBoxImage; }
        else { dialogueBox.transform.GetChild(0).GetComponent<Image>().sprite = DEFAULT_DIALOGUE_BOX_IMAGE; }

        //Set speaker protrait image
        if (observations[currSentence].speakerPortrait != null) { playerResponseBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = observations[currSentence].speakerPortrait; }
        else { playerResponseBox.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = DEFAULT_PLAYER_SPEAKER_SPRITE; }

        //audSource.ignoreListenerPause = true;
        //audSource.ignoreListenerVolume = true;

        //Writes out the text character by character with selected settings
        for (int i = 1; i <= fullText.Length; i++)
        {
            int sound = Random.Range(0, talkSound.Length);
            dialogueText.text = fullText;
            currText = dialogueText.text.Insert(i, "<color=#00000000>");
            dialogueText.text = currText;

            if (!char.IsWhiteSpace(fullText[i - 1])) { AudioManager.Instance.dialogueSource.PlayOneShot(talkSound[sound], 1); }
            if (talkSpeed == DEFAULT_TALK_SPEED)
            { talkSpeed *= PlayerPrefs.GetInt("Text Speed", 2); }
            float waitTime = 1 / (talkSpeed * 5);
            yield return new WaitForSeconds(waitTime);
        }

        //audSource.ignoreListenerPause = false;
        //audSource.ignoreListenerVolume = false;
        speaking = false;
        StartCoroutine(ArrowBlink());
    }

    private IEnumerator Scribble() {
        Debug.Log("Do");
        AudioManager.Instance.sfxSource.PlayOneShot(clueFound, 1);
        dialogueBox.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1.0f);
        dialogueBox.transform.GetChild(2).gameObject.SetActive(false);
    }
    private IEnumerator ArrowBlink()
    {
        if (observations[currSentence].isVerbalClue) { StartCoroutine(Scribble()); }
        while (true)
        {
            arrow.SetActive(true);
            yield return new WaitForSecondsRealtime(0.2f);
            arrow.SetActive(false);
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    private void EndConversation()
    {
        //currSentence = 0;
        dialogueBox.SetActive(false);
        playerResponseBox.transform.GetChild(1).gameObject.SetActive(false);
        dialogueBox.transform.GetChild(2).gameObject.SetActive(false);
        isTalking = false;
        Destroy(this.gameObject);
    }
    public void MakeObservation() {
        //Debug.Log(observations.Length);
        clicked = true;
        if (observations.Length > 0 && observations != null)
        {
            //Debug.Log("Do");
            StartCoroutine(WriteText());
        }
        else
        {
            //Debug.Log("Don't");
            EndConversation();
        }
    }
}
