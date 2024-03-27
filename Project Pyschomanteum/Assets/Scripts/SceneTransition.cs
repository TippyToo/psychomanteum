using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    [Tooltip("Set to require input to transfer scenes. False automatically changes scenes when the player gets close enough")]
    public bool interactable;
    public string sceneNameToLoad = "MainMenu";
    public bool loadingSubArea;
    private bool detectsPlayer = false;


    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) {
            LoadNextArea(loadingSubArea);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = true;
            if (!interactable) {
                LoadNextArea(loadingSubArea);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = false;
        }
    }

    private void LoadNextArea(bool sub) {
        GameObject.Find("Save Manager").GetComponent<SaveManager>().inSubWorld = sub;
        GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
        SceneManager.LoadScene(sceneNameToLoad);
    }
}
