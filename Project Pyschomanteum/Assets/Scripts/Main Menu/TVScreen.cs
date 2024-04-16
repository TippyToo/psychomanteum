using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TVScreen : MonoBehaviour
{
    Stack<GameObject> tvScreen = new Stack<GameObject>();
    private int saveSlots = 3;

    // Settings variables
    public Slider musicVolumeSlider;
    public Slider dialogueVolumeSlider;
    public Slider textSpeedSlider;
    public Text musicText;
    public Text dialogueText;
    public Text textSpeedText;


    // Stack functions
    public void Push(GameObject screen)
    {
        tvScreen.Push(screen);
        screen.SetActive(true);
    }

    public void Pop()
    {
        tvScreen.Pop().SetActive(false);
    }

    public void PopAll()
    {
        for (int i = 0; i < tvScreen.Count; i++) { Pop(); }
    }


    public void Quit()
    {
        Application.Quit();
    }


    // Save selection functions
    public void DecideLoadButtonBehaviors()
    {
        for (int i = 0; i < saveSlots; i++)
        {
            transform.GetChild(1).GetChild(i).GetComponent<SaveSlot>().DecideButtonBehavior();
        }
    }


    // Settings functions

    // "category" should be the one word, capitalized volume type this slider adjusts. i.e. "Music"
    public void SaveVolume(string category)
    {
        switch (category)
        {
            case "Music":
                PlayerPrefs.SetFloat(category, musicVolumeSlider.value);
                musicText.text = ((int)(musicVolumeSlider.value * 100)).ToString() + "%";
                break;
            case "Dialogue":
                PlayerPrefs.SetFloat(category, dialogueVolumeSlider.value);
                dialogueText.text = ((int)(dialogueVolumeSlider.value * 100)).ToString() + "%";
                break;
        }
    }

    public void SaveTextSpeed()
    {
        // Save setting
        PlayerPrefs.SetInt("Text Speed", ((int)textSpeedSlider.value));

        // Change displayed value
        string temp = "";
        switch (textSpeedSlider.value)
        {
            case 1:
                temp = "Slow";
                break;
            case 2:
                temp = "Medium";
                break;
            case 3:
                temp = "Fast";
                break;
        }
        textSpeedText.text = temp;
    }
}
