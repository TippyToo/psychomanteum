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
    public Slider volumeSlider;
    public Slider textSpeedSlider;
    private List<ISettings> settingsObjList;
    public Text volumeText;
    public Text textSpeedText;


    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
       if (volumeSlider == null) volumeSlider = GameObject.Find("Volume Slider").GetComponent<Slider>();
       if (textSpeedSlider == null) textSpeedSlider = GameObject.Find("Text Speed Slider").GetComponent<Slider>();
       if (volumeText == null) volumeText = GameObject.Find("Volume Value").GetComponent<Text>();
       if (textSpeedText == null) textSpeedText = GameObject.Find("Text Speed Value").GetComponent<Text>();
        settingsObjList = FindAllSettingsObj();
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

    

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
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

    public void UpdateValues()
    {
        volumeText.text = ((int) (volumeSlider.value*100)).ToString() + "%";
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
