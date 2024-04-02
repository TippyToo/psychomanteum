using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TVScreen : MonoBehaviour
{
    Stack<GameObject> tvScreen = new Stack<GameObject>();
    private int sceneToLoad = 1;

    // Stack functions
    public void Push(GameObject screen)
    {
        tvScreen.Push(screen);
        screen.SetActive(true);
    }

    public void Pop()
    {
        tvScreen.Pop().SetActive(false);
    }

    public void PopAll()
    {
        for (int i = 0; i < tvScreen.Count; i++) { Pop(); }
    }


    // Button functions
    public void Play()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
