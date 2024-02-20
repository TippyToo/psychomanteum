using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void Start() {
        paused = false;
        homePage = transform.Find("Home Page").gameObject;
        cluesPage = transform.Find("Clues Page").gameObject;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        saveManager = GameObject.Find("Save Manager").GetComponent<SaveManager>();
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
    private void Pause() {
        //Pauses or unpauses the game
        paused = !paused;
        if (paused) {
            Time.timeScale = 0;
            if (!player.talking){
                if (lastOpened != null) { PushSection(lastOpened); }
                else { PushSection(homePage); }
            } else {
                PushSection(cluesPage);
            }
        } else {
            CloseJournal();
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
        PopAll();
        PushSection(section);
    }
    public void PopSection() {
        //Remove current section
        sections.Peek().SetActive(false);
        sections.Pop();
    }
    public void CloseJournal() {
        lastOpened = sections.Peek();
        PopAll();
        Time.timeScale = 1;
    }
    private void PopAll()
    {
        //"Bookmarks" the current page then closes all windows
        for (int i = 0; i < sections.Count; i++) { PopSection(); }
    } 
}
