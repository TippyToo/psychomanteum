using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    private AudioManager audioManager;
    private AudioMixer audioMixer;
    
    private Slider textSpeedSlider;
    private Dropdown resolutionDropdown;
    private Toggle fsToggle;

    private Resolution[] resolutions;

    bool loaded = false;

    private float MIN_VOLUME = 80; //Absolute value of minimum slider value. MUST BE POSOTIVE

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        audioMixer = audioManager.audioMixer;
        
    }
    #region Audio
    public void SetMaster(float volume) {
        if (loaded)
        {
            audioMixer.SetFloat("master", volume);
            PlayerPrefs.SetFloat("master", volume);
            audioManager.masterText.text = ((int)(((MIN_VOLUME - Mathf.Abs(volume)) / MIN_VOLUME) * 100)).ToString() + "%";
        }
    }
    public void SetDialogue(float volume)
    {
        if (loaded)
        {
            audioMixer.SetFloat("dialogue", volume);
            audioManager.dialogueText.text = ((int)(((MIN_VOLUME - Mathf.Abs(volume)) / MIN_VOLUME) * 100)).ToString() + "%";
            PlayerPrefs.SetFloat("dialogue", volume);
        }
    }
    public void SetMusic(float volume)
    {
        if (loaded)
        {
            audioMixer.SetFloat("music", volume);
            audioManager.musicText.text = ((int)(((MIN_VOLUME - Mathf.Abs(volume)) / MIN_VOLUME) * 100)).ToString() + "%";
            PlayerPrefs.SetFloat("music", volume);
        }
    }
    public void SetSFX(float volume)
    {
        if (loaded)
        {
            audioMixer.SetFloat("sfx", volume);
            audioManager.sfxText.text = ((int)(((MIN_VOLUME - Mathf.Abs(volume)) / MIN_VOLUME) * 100)).ToString() + "%";
            PlayerPrefs.SetFloat("sfx", volume);
        }
    }
    #endregion

    public void SaveTextSpeed(Slider self)
    {
        PlayerPrefs.SetInt("Text Speed", (int) self.value);
        UpdateTextSpeedValue(self);
    }

    public void FullscreenToggle(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen)
            PlayerPrefs.SetInt("fullscreen", 1);
        else
            PlayerPrefs.SetInt("fullscreen", 0);
    }

    public void GetResolutions(int width, int height) {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int current = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            options.Add(resolutions[i].width + "x" + resolutions[i].height);

            if (resolutions[i].width == width &&
                resolutions[i].height == height) 
            { 
                current = i;
                PlayerPrefs.SetInt("width", resolutions[i].width);
                PlayerPrefs.SetInt("height", resolutions[i].height);
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = current;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int i) {
        Resolution res = resolutions[i];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("width", res.width);
        PlayerPrefs.SetInt("height", res.height);
    }

    public void LoadValues() {

        if (FindObjectOfType<JournalManager>() != null)
        {
            GameObject generalSettings = FindObjectOfType<JournalManager>().gameObject.transform.GetChild(4).GetChild(5).gameObject;
            resolutionDropdown = generalSettings.transform.GetChild(3).GetComponent<Dropdown>();
            fsToggle = generalSettings.transform.GetChild(2).GetComponent<Toggle>();
            textSpeedSlider = generalSettings.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
        }
        else {
            GameObject generalSettings = FindObjectOfType<TVScreen>().gameObject.transform.GetChild(2).GetChild(4).gameObject;
            resolutionDropdown = generalSettings.transform.GetChild(3).GetComponent<Dropdown>();
            fsToggle = generalSettings.transform.GetChild(4).GetComponent<Toggle>();
            textSpeedSlider = generalSettings.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
        }

        int fs = PlayerPrefs.GetInt("fullscreen", 1);
        if (fs > 0) { 
            fsToggle.isOn = true; 
            FullscreenToggle(true); 
        }
        else { 
            fsToggle.isOn = false; 
            FullscreenToggle(false); 
        }
        

        int width = PlayerPrefs.GetInt("width", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("height", Screen.currentResolution.height);
        GetResolutions(width, height);

        float masterVolume = PlayerPrefs.GetFloat("master", 0);
        audioManager.masterText.text = ((int)(((MIN_VOLUME - Mathf.Abs(masterVolume)) / MIN_VOLUME) * 100)).ToString() + "%";

        float musicVolume = PlayerPrefs.GetFloat("music", 0);
        audioManager.musicText.text = ((int)(((MIN_VOLUME - Mathf.Abs(musicVolume)) / MIN_VOLUME) * 100)).ToString() + "%";

        float dialogueVolume = PlayerPrefs.GetFloat("dialogue", 0);
        audioManager.dialogueText.text = ((int)(((MIN_VOLUME - Mathf.Abs(dialogueVolume)) / MIN_VOLUME) * 100)).ToString() + "%";

        float sfxVolume = PlayerPrefs.GetFloat("sfx", 0);
        audioManager.sfxText.text = ((int)(((MIN_VOLUME - Mathf.Abs(sfxVolume)) / MIN_VOLUME) * 100)).ToString() + "%";

        textSpeedSlider.value = PlayerPrefs.GetInt("Text Speed", 2);
        UpdateTextSpeedValue(textSpeedSlider);

        loaded = true;
    }

    private void UpdateTextSpeedValue(Slider self)
    {
        Text text = self.transform.parent.GetChild(1).GetComponent<Text>();
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
        text.text = temp;
    }
}
