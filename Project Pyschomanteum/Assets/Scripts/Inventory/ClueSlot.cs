using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ClueSlot : MonoBehaviour
{
    //Behaviour for inventory slots. Populates and depopulates

    [HideInInspector]
    public VerbalClueData clue = new VerbalClueData();

    public void OnClick()
    {
        if (clue.description != null)
        {
            GameObject.Find("Clue Description").GetComponent<Text>().text = clue.description;
            GameObject.Find("UI").transform.GetChild(0).GetComponent<JournalManager>().PopulatePresentButton(clue);
        }
    }

    public void OnExit()
    {
        GameObject.Find("Item Description").GetComponent<Text>().text = "";
    }
}
