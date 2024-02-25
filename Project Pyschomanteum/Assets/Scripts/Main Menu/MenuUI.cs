using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private Stack<GameObject> menuUI = new Stack<GameObject>();
    private int sceneToLoad = 1;

    //Settings variables

    //Volume settings
    public Slider dialogueVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambienceVolumeSlider;
    public Slider SFXVolumeSlider;

    public Slider textSpeedSlider;
    private List<ISettings> settingsObjList;


    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        settingsObjList = FindAllSettingsObj();
        dialogueVolumeSlider.value = PlayerPrefs.GetFloat("Volume");
        textSpeedSlider.value = PlayerPrefs.GetInt("Text Speed");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel")) { Pop(); }
    }
    public void Play() {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Push(GameObject uiSection)
    {
        //Add specified section
        menuUI.Push(uiSection);
        uiSection.SetActive(true);
    }
    public void Pop()
    {
        //Remove current section
        menuUI.Peek().SetActive(false);
        menuUI.Pop();
    }

    public void PopAll()
    {
        //"Bookmarks" the current page then closes all windows
        for (int i = 0; i < menuUI.Count; i++) { Pop(); }
    }

    public void DecideLoadButtonBehaviors() {
        for (int i = 0; i < 3; i++) {
            GameObject.Find("Load Save").transform.GetChild(i).GetComponent<SaveSlot>().DecideButtonBehavior();
        }
    }

    
    // "category" should be the one word, capitalized volume type this slider adjusts. i.e. "Music"
    public void SaveVolume(string category)
    {
        PlayerPrefs.SetFloat(category, dialogueVolumeSlider.value);
    }

    public void SaveTextSpeed()
    {
        PlayerPrefs.SetInt("Text Speed", textSpeedSlider.value.ConvertTo<int>());
    }

    private void ApplyAllChanges()
    {
        foreach (ISettings settingsObj in settingsObjList)
        {
            settingsObj.ApplySettings();
        }
    }

    private List<ISettings> FindAllSettingsObj()
    {
        IEnumerable<ISettings> settingsObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISettings>();
        return new List<ISettings>(settingsObjects);
    }

    // Input paramter "setting" should be the slider gameobject of the calling slider
    // This function currently relies on hierarchy order (slider then text object), which is not ideal but I'll fix it when I can think of a better way.
    // For now I'll just leave a warning in if it looks like they are not in the right order

    // omg I'm an idiot

    // I'll fix it later jesus christ I'm dense
    public void UpdateVolumeValue(Slider setting)
    {
        // Hierarchy order warning
        if (setting.transform.GetSiblingIndex() != 0 || setting.transform.parent.childCount != 2) {
            Debug.LogError("Illegal hierarchy order of " + setting.transform.parent.name +
            "\nThe parent object must have exactly two children. Slider first, Text second.");
        }


        Text valueText = setting.transform.parent.GetChild(1).GetComponent<Text>();
        valueText.text = ((int) (dialogueVolumeSlider.value*100)).ToString() + "%";   
    }

    public void UpdateTextSpeedValue(Slider setting)
    {
        // Hierarchy order warning
        if (setting.transform.GetSiblingIndex() != 0 || setting.transform.parent.childCount != 2)
        {
            Debug.LogError("Illegal hierarchy order of " + setting.transform.parent.name +
            "\nThe parent object must have exactly two children. Slider first, Text second.");
        }


        Text valueText = setting.transform.parent.GetChild(1).GetComponent<Text>();
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
        valueText.text = temp;
    }
}
