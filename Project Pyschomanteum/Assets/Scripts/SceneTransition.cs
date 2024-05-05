using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneTransition : MonoBehaviour
{
    [Tooltip("Set to require input to transfer scenes. False automatically changes scenes when the player gets close enough")]
    public bool interactable;
    public string sceneNameToLoad = "MainMenu";
    public bool loadingSubArea;
    private bool detectsPlayer = false;
    public bool unlocked;
    public float fadeTimer;
    private SpriteRenderer fade;

    private void Start()
    {
        fade = FindObjectOfType<Fade>().GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) {
            StartCoroutine(FadeOut());
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = true;
            if (!interactable) {
                StartCoroutine(FadeOut());
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = false;
        }
    }

    public IEnumerator FadeOut()
    {
        if (unlocked)
        {
            FindObjectOfType<PlayerController>().canInteract = false;
            Color color = fade.color;
            while (color.a < 1.0f)
            {
                color.a += Time.deltaTime / fadeTimer;
                if (color.a >= 1.0f)
                    color.a = 1.0f;
                fade.color = color;
                yield return null;
            }
            LoadNextArea(loadingSubArea);
        }
    }
    private void LoadNextArea(bool sub) {
        if (unlocked)
        {
            GameObject.Find("Save Manager").GetComponent<SaveManager>().loadingSubWorld = sub;
            GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }
}
