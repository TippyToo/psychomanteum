using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Stack<GameObject> sections = new Stack<GameObject>();
    private Canvas UI;
    void Start()
    {
        UI = GameObject.Find("UI").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PushSection(GameObject section)
    {
        
        sections.Push(section);
        section.SetActive(true);

    }
    public void PopSection()
    {
        if (sections.Count > 1 )
        {

            sections.Peek().SetActive(false);
            sections.Pop();
        }
        

    }
    public void PopAll() {
        for (int i = 1; i < sections.Count; i++) { PopSection(); }
    }
}
