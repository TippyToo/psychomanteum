using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.Burst.Intrinsics;
using UnityEngine.Device;

public class SaveSlot : MonoBehaviour
{
    public int saveSlot;
    private SaveManager saveManager;
    private bool saveDataFound;
    private TVScreen menu;
    public GameObject deleteSavePrompt;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        menu = FindObjectOfType<TVScreen>(); ;
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
            transform.GetChild(0).GetComponent<Text>().text = "Slot " + (saveSlot + 1) + ":\n" + "Chapter " + saveManager.saveData[saveSlot].chapter;
        }
        else 
        {
            transform.GetComponent<Button>().onClick.RemoveListener(LoadGame);
            transform.GetComponent<Button>().onClick.AddListener(NewGame);
            transform.GetChild(0).GetComponent<Text>().text = "Slot " + (saveSlot + 1) + ":\n" + "New Game";
        }

    }


    public void DeleteSavePrompt()
    {
        if (saveDataFound)
        {
            menu.Push(deleteSavePrompt);
            Button yes = GameObject.Find("Yes").GetComponent<Button>();
            GameObject.Find("Delete Warning").GetComponent<Text>().text = "!!WARNING!!\nYou are about to delete all save data in slot " + (saveSlot + 1) + ".\nAre you sure you want to continue?";
            yes.onClick.RemoveAllListeners();
            yes.onClick.AddListener(DeleteSave);
            yes.onClick.AddListener(menu.Pop);
            Debug.Log("Data to be deleted" + saveSlot);
        }
    }
    public void DeleteSave() {
        saveManager.DeleteSave(saveSlot);
        DecideButtonBehavior();
    }

    private void LoadGame() {
        saveManager.LoadGame(saveSlot);
        SceneManager.LoadScene(saveManager.saveData[saveSlot].sceneToLoad);
    }
    private void NewGame() {
        saveManager.NewGame(saveSlot);
        SceneManager.LoadScene(saveManager.saveData[saveSlot].sceneToLoad);
    }
}
