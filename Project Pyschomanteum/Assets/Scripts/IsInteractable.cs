using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IsInteractable : MonoBehaviour
{

    public float fadeTime;
    [HideInInspector]
    public bool detectsPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Interact indicator fade in
    private IEnumerator FadeIn(SpriteRenderer interactSprite)
    {
        Color temp = interactSprite.color;
        while (temp.a < 1.0f)
        {
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
        if (other.tag == "Player")
        {
            if (transform.parent != null && transform.parent.GetComponent<Dialogue>() != null && transform.parent.GetComponent<Dialogue>().player == null) {
                transform.parent.GetComponent<Dialogue>().player = other.GetComponent<PlayerController>();
            }
            detectsPlayer = true;
            StopAllCoroutines();
            StartCoroutine(FadeIn(GetComponent<SpriteRenderer>()));
        }
        if (transform.parent != null && transform.parent.GetComponent<Dialogue>() != null) {
            transform.parent.GetComponent<Dialogue>().SpeakOnProximity();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            detectsPlayer = false;
            StopAllCoroutines();
            StartCoroutine(FadeOut(GetComponent<SpriteRenderer>()));
        }
        
    }
}
