using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    private bool journalOpen;
    private const string journalName = "JournalUI";
    // Start is called before the first frame update
    void Start()
    {
        journalOpen = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Journal"))
        {
            if (journalOpen) { Close(); }
            else { Close(); }
        }
    }

    public void Open() { transform.Find(journalName).gameObject.SetActive(true); }

    public void Close() { transform.Find(journalName).gameObject.SetActive(false); }
}
