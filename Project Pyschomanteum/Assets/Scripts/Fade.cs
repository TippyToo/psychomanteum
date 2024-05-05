using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private SpriteRenderer image;
    private PlayerController playerCont;
    public float fadeTimer;

    // Start is called before the first frame update
    void Start() {
        image = GetComponent<SpriteRenderer>();
        playerCont = FindObjectOfType<PlayerController>();
        StartCoroutine(FadeIn()); 
    }

    public IEnumerator FadeIn() {
        Color color = image.color;
        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / fadeTimer;
            if (color.a <= 0.0f)
                color.a = 0.0f;
            image.color = color;
            yield return null;
        }
        playerCont.canInteract = true;
    }
    public IEnumerator FadeOutThenIn(GameObject player, Vector3 warpLocation)
    {
        playerCont.canInteract = false;
        Color color = image.color;
        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime / fadeTimer;
            if (color.a >= 1.0f)
                color.a = 1.0f;
            image.color = color;
            yield return null;
        }
        player.transform.position = warpLocation;
        while (color.a > 0.0f)
        {
            color.a -= Time.deltaTime / fadeTimer;
            if (color.a <= 0.0f)
                color.a = 0.0f;
            image.color = color;
            yield return null;
        }
        playerCont.canInteract = true;
    }
    public IEnumerator FadeOutLoadScene(string sceneToLoad)
    {
        playerCont.canInteract = false;
        Color color = image.color;
        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime / fadeTimer;
            if (color.a >= 1.0f)
                color.a = 1.0f;
            image.color = color;
            yield return null;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
