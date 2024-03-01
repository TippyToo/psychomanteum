using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveMusicVolume(Slider self)
    {
        PlayerPrefs.SetFloat("Music", self.value);
    }

    public void SaveDialogueVolume(Slider self)
    {
        PlayerPrefs.SetFloat("Dialogue", self.value);
    }

    public void SaveAmbienceVolume(Slider self)
    {
        PlayerPrefs.SetFloat("Ambience", self.value);
    }

    public void SaveSFXVolume(Slider self)
    {
        PlayerPrefs.SetFloat("SFX", self.value);
    }

    public void SaveTextSpeed(Slider self)
    {
        PlayerPrefs.SetInt("Text Speed", (int) self.value);
    }
}
