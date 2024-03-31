using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class JournalManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Stack<GameObject> sections = new Stack<GameObject>();
    private bool paused;
    private GameObject homePage;
    private GameObject cluesPage;
    private GameObject lastOpened = null;
    private PlayerController player;
    private SaveManager saveManager;
    [HideInInspector]
    public bool isOpen = false;
    public bool canOpen = true;
    public bool canClose = true;

    public bool isPresenting = false;

    public GameObject chapterTabs;

    public ItemInspection itemInspector;
    public Text itemDescription;
    public Text clueDescription;

    void Start() {
        paused = false;
        homePage = transform.Find("Home Page").gameObject;
        cluesPage = transform.Find("Clues Page").gameObject;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        saveManager = GameObject.Find("Save Manager").GetComponent<SaveManager>();
        SetJournalChapterAccess();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonUp("Journal")) { Pause(); }

    }
    public void Resume() { Pause(); }
    public void SaveAndQuit() {
        //Add saving later
        Time.timeScale = 1;
        saveManager.SaveGame();
        SceneManager.LoadScene(0); 
    }
    public void Pause() {
        //Pauses or unpauses the game
        paused = !paused;
        if (paused) {
            if (canOpen) { 
                Time.timeScale = 0;
                //GameObject.Find("Inventory Manager").GetComponent<InventoryManager>().UpdateInventory();
                isOpen = true;
                ShowChapterTabs();
                if (player.canMove){
                    if (lastOpened != null) { PushSection(lastOpened); }
                    else { PushSection(homePage); }
                } else {
                    PushSection(cluesPage);
                }
            }
        } else {
            if (canClose) {
                if (isOpen) { CloseJournal(); }
            }
        }
    }
    public bool IsPaused() { return paused; }
    public void PushSection(GameObject section) {
        //Add specified section
        sections.Push(section);
        section.SetActive(true);
    }
    public void PushNewJournalSection(GameObject section) {
        //Add specified section
        if (section.name == "Home Page") {
            if (isPresenting) { transform.GetChild(6).GetComponent<PresentClue>().Cancel(); }
        } else { 
            PopAll();
            PushSection(section);
        }
    }
    public void PopSection() {
        //Remove current section
        sections.Peek().SetActive(false);
        sections.Pop();
    }
    public void CloseJournal() {
        lastOpened = sections.Peek();
        GameObject responseButton = transform.GetChild(6).gameObject; // Set response button to not show
        responseButton.GetComponent<PresentClue>().OnClose();
        PopAll();
        HideChapterTabs();
        isOpen = false;
        paused = false;
        itemInspector.DeleteInspectedObject();
        itemDescription.text = "";
        clueDescription.text = "";
        Time.timeScale = 1;
    }
    public void EnableJournalBehaviour() {
        canOpen = true;
        canClose = true;
    }
    public void DisableJournalBehaviour()
    {
        canOpen = false;
        canClose = false;
    }
    private void PopAll()
    {
        //"Bookmarks" the current page then closes all windows
        for (int i = 0; i < sections.Count; i++) { PopSection(); }
    }
    #region Present Button
    public void PopulatePresentButton(ItemData item = null) {
        if (isPresenting) {
            GameObject button = transform.GetChild(6).gameObject;
            button.SetActive(true);
            if (item != null) { button.GetComponent<PresentClue>().itemToPresent = item; }
        }
    }
    public void PopulatePresentButton(VerbalClueData vClue = null)
    {
        if (isPresenting)
        {
            GameObject button = transform.GetChild(6).gameObject;
            button.SetActive(true);
            if (vClue != null) { button.GetComponent<PresentClue>().clueData = vClue; }
        }
    }
    #endregion

    //Changes the amount of chapters you have access to in your journal based on the level
    public void SetJournalChapterAccess()
    {
        int level = GameObject.Find("Level Manager").GetComponent<LevelManager>().level;
        if (level > 0)
        {
            Transform temp = transform.GetChild(5);
            for (int i = 0; i < level && level != 0; i++)
            {
                temp.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = level + 1; i < temp.childCount && level != temp.childCount; i++)
            {
                temp.GetChild(i).gameObject.SetActive(false);
            }
        }
        else {
            Transform temp = transform.GetChild(5);
            for (int i = 0; i < temp.childCount; i++)
            {
                temp.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void HideChapterTabs() { chapterTabs.SetActive(false); }

    private void ShowChapterTabs() { chapterTabs.SetActive(true); }
}
