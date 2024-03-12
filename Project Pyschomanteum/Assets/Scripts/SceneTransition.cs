using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    [Tooltip("Set to require input to transfer scenes. False automatically changes scenes when the player gets close enough")]
    public bool interactable;
    public string sceneNameToLoad = "MainMenu";
    
    private bool detectsPlayer = false;


    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) {
            GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = true;
            if (!interactable) {
                GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = false;
        }
    }
}
