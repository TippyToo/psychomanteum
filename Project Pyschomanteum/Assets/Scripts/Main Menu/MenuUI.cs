using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    private Stack<GameObject> menuUI = new Stack<GameObject>();
    private int sceneToLoad = 1;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Cancel")) { Pop(); }
    }
    public void Play() {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Push(GameObject uiSection)
    {
        //Add specified section
        menuUI.Push(uiSection);
        uiSection.SetActive(true);
    }
    public void Pop()
    {
        //Remove current section
        menuUI.Peek().SetActive(false);
        menuUI.Pop();
    }

    public void PopAll()
    {
        //"Bookmarks" the current page then closes all windows
        for (int i = 0; i < menuUI.Count; i++) { Pop(); }
    }

    public void DecideLoadButtonBehaviors() {
        for (int i = 0; i < 3; i++) {
            GameObject.Find("Load Save").transform.GetChild(i).GetComponent<SaveSlot>().DecideButtonBehavior();
        }
    }
}
