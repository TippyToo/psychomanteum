using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TVScreen : MonoBehaviour
{
    Stack<GameObject> tvScreen = new Stack<GameObject>();


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


    public void Quit()
    {
        Application.Quit();
    }

    public void DecideLoadButtonBehaviors()
    {
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(0).GetChild(i).GetComponent<SaveSlot>().DecideButtonBehavior();
        }
    }

}
