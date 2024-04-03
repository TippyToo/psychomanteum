using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    int hour;
    int minute;
    bool isPM;

    public GameObject amSlot;
    public GameObject pmSlot;
    public Image hourSlot1;
    public Image hourSlot2;
    public Image minuteSlot1;
    public Image minuteSlot2;

    public Sprite[] digitSprites = new Sprite[10];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateClock();
    }

    void UpdateClock()
    {
        DateTime time = DateTime.Now;

        hour = time.Hour;
        if (hour >= 12) isPM = true;
        if (hour > 12) hour -= 12;
        minute = time.Minute;

        // Assign AM/PM
        amSlot.SetActive(!isPM);
        pmSlot.SetActive(isPM);

        // Assign hour slots
        if (hour > 9)
        {
            hourSlot1.gameObject.SetActive(true);
            hourSlot1.sprite = digitSprites[1];
            hourSlot2.sprite = digitSprites[hour-10];
        }
        else if (hour == 0)
        {
            hourSlot1.sprite = digitSprites[1];
            hourSlot2.sprite = digitSprites[2];
        }
        else
        {
            hourSlot1.gameObject.SetActive(false);
            hourSlot2.sprite = digitSprites[hour];
        }

        // Assign minute slots
        minuteSlot1.sprite = digitSprites[minute / 10];
        minuteSlot2.sprite = digitSprites[minute % 10];
    }

    public void PrintTime()
    {
        UpdateClock();

        string output = hour + ":" + minute;
        if (isPM) { output += "PM"; } else { output += "AM"; }
        print(output);
    }
}
