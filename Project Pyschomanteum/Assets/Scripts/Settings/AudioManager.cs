using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioMixer audioMixer;

    [HideInInspector] public AudioSource musicSource;
    [HideInInspector] public AudioSource sfxSource;
    [HideInInspector] public AudioSource dialogueSource;

    private Slider master;
    private Slider music;
    private Slider dialogue;
    private Slider sfx;

    [HideInInspector] public Text masterText;
    [HideInInspector] public Text musicText;
    [HideInInspector] public Text dialogueText;
    [HideInInspector] public Text sfxText;


    private void Awake()
    {
        if (Instance != null) { Debug.Log("More than one audio manager. Destroying new one."); Destroy(this.gameObject); return; }
        Instance = this;
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        sfxSource = transform.GetChild(1).GetComponent<AudioSource>();
        dialogueSource = transform.GetChild(2).GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadVolume();
        LoadValues();
    }

    private void LoadValues() {
        if (FindObjectOfType<JournalManager>() != null)
        {
            GameObject volumeSettings = FindObjectOfType<JournalManager>().transform.GetChild(4).GetChild(4).gameObject;

            master = volumeSettings.transform.GetChild(1).GetChild(0).GetComponent<Slider>();
            master.value = PlayerPrefs.GetFloat("master", 0);

            music = volumeSettings.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
            music.value = PlayerPrefs.GetFloat("music", 0);

            dialogue = volumeSettings.transform.GetChild(3).GetChild(0).GetComponent<Slider>();
            dialogue.value = PlayerPrefs.GetFloat("dialogue", 0);

            sfx = volumeSettings.transform.GetChild(4).GetChild(0).GetComponent<Slider>();
            sfx.value = PlayerPrefs.GetFloat("sfx", 0);
        }
        else {
            GameObject volumeSettings = FindObjectOfType<TVScreen>().gameObject.transform.GetChild(2).GetChild(3).gameObject;

            master = volumeSettings.transform.GetChild(2).GetChild(0).GetComponent<Slider>();
            master.value = PlayerPrefs.GetFloat("master", 0);

            music = volumeSettings.transform.GetChild(3).GetChild(0).GetComponent<Slider>();
            music.value = PlayerPrefs.GetFloat("music", 0);

            dialogue = volumeSettings.transform.GetChild(4).GetChild(0).GetComponent<Slider>();
            dialogue.value = PlayerPrefs.GetFloat("dialogue", 0);

            sfx = volumeSettings.transform.GetChild(5).GetChild(0).GetComponent<Slider>();
            sfx.value = PlayerPrefs.GetFloat("sfx", 0);
        }
        masterText = master.transform.parent.GetChild(1).GetComponent<Text>();
        musicText = music.transform.parent.GetChild(1).GetComponent<Text>();
        dialogueText = dialogue.transform.parent.GetChild(1).GetComponent<Text>();
        sfxText = sfx.transform.parent.GetChild(1).GetComponent<Text>();
        GetComponent<SettingsManager>().LoadValues();
    }
    private void LoadVolume()
    {
        float masterV = PlayerPrefs.GetFloat("master", 0);
        audioMixer.SetFloat("master", masterV);

        float musicV = PlayerPrefs.GetFloat("music", 0);
        audioMixer.SetFloat("music", musicV);

        float dialogueV = PlayerPrefs.GetFloat("dialogue", 0);
        audioMixer.SetFloat("dialogue", dialogueV);

        float sfxV = PlayerPrefs.GetFloat("sfx", 0);
        audioMixer.SetFloat("sfx", sfxV);
    }
}
