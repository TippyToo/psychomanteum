using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    [Tooltip("Set to require input to transfer scenes. False automatically changes scenes when the player gets close enough")]
    public bool interactable;
    public string sceneNameToLoad = "MainMenu";
    public float fadeTime;
    private bool detectsPlayer = false;


    // Update is called once per frame
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact")) {
            GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }

    //Interact indicator fade in
    private IEnumerator FadeIn(SpriteRenderer interactSprite) {
        Color temp = interactSprite.color;
        while (temp.a < 1.0f) {
            temp.a += Time.deltaTime / fadeTime;
            if (temp.a >= 1.0f)
                temp.a = 1.0f;
            interactSprite.color = temp;
            yield return null;
        }
    }

    //Interact indicator fade out
    private IEnumerator FadeOut(SpriteRenderer interactSprite)
    {
        Color temp = interactSprite.color;
        while (temp.a > 0.0f)
        {
            temp.a -= Time.deltaTime / fadeTime;
            if (temp.a <= 0.0f)
                temp.a = 0.0f;
            interactSprite.color = temp;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = true;
            if (interactable) {
                StopAllCoroutines();
                StartCoroutine(FadeIn(GetComponent<SpriteRenderer>()));
            } else if (!interactable) {
                GameObject.Find("Save Manager").GetComponent<SaveManager>().SaveGame();
                SceneManager.LoadScene(sceneNameToLoad);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { 
            detectsPlayer = false;
            if (interactable)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOut(GetComponent<SpriteRenderer>()));
            }
        }
    }
}
