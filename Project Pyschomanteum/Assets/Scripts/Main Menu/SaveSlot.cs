using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveSlot : MonoBehaviour
{
    public int saveSlot;
    private SaveManager saveManager;
    private bool saveDataFound;
    private MenuUI menu;
    public GameObject deleteSavePrompt;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = GameObject.Find("Save Manager").GetComponent<SaveManager>();
        menu = GameObject.Find("Main Menu").GetComponent<MenuUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DecideButtonBehavior() {
        saveManager = GameObject.Find("Save Manager").GetComponent<SaveManager>();
        transform.GetComponent<Button>().onClick.RemoveAllListeners();
        if (saveManager.saveData[saveSlot] != null) 
        { saveDataFound = true; } 
        else 
        { saveDataFound = false; }
        if (saveDataFound)
        {
            transform.GetComponent<Button>().onClick.RemoveListener(NewGame);
            transform.GetComponent<Button>().onClick.AddListener(LoadGame);
            transform.GetChild(0).GetComponent<Text>().text = "Ch: " + saveManager.saveData[saveSlot].chapter;
        }
        else 
        {
            transform.GetComponent<Button>().onClick.RemoveListener(LoadGame);
            transform.GetComponent<Button>().onClick.AddListener(NewGame);
            transform.GetChild(0).GetComponent<Text>().text = "New Game";
            Debug.Log("NewSave");
        }
        //transform.GetComponent<Image>().sprite = saveManager.saveData[saveSlot].saveImage;

    }

    public void DeleteSavePrompt()
    {
        menu.Push(deleteSavePrompt);
        Button yes = GameObject.Find("Yes").GetComponent<Button>();
        yes.onClick.RemoveListener(DeleteSave);
        yes.onClick.AddListener(DeleteSave);
        Debug.Log("Data to be deleted" + saveSlot);
    }
    public void DeleteSave() {
        saveManager.DeleteSave(saveSlot);
        DecideButtonBehavior();
    }

    private void LoadGame() {
        saveManager.LoadGame(saveSlot);
    }
    private void NewGame() {
        saveManager.NewGame(saveSlot);
    }
}
