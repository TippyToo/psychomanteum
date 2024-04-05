using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TapeMover : MonoBehaviour
{
    public Transform slotsContainer;
    private Vector3[] slots = new Vector3[5];
    [HideInInspector]
    public Tape activeTape;
    private TVScreen screen;

    private float tapeMoveSpeed = 200; 

    // Start is called before the first frame update
    void Start()
    {
        // Gather vectors for all slots
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotsContainer.GetChild(i).position;
        }

        screen = FindObjectOfType<TVScreen>();
    }

    private void Update()
    {
        
    }
    public void MoveTape(Tape tape)
    {
        
        if (tape == activeTape)
        {
            DeactivateTape(tape);
        }
        else
        {
            if (activeTape != null) { DeactivateTape(activeTape); }
            ActivateTape(tape);
        }

    }

    void ActivateTape(Tape tape) 
    {
        int timer = 0;
        while (tape.transform.position != slots[0] && ++timer < 1000)
        {
            tape.transform.position = Vector3.MoveTowards(tape.transform.position, slots[0], tapeMoveSpeed);
            print("movin " + timer);
        }
        tape.transform.position = slots[0];
        activeTape = tape;
        screen.Push(tape.associatedScreen);
    }

    void DeactivateTape(Tape tape) 
    {
        int timer = 0;
        while (tape.transform.position != slots[tape.homeSlot] && ++timer < 1000)
        {
            tape.transform.position = Vector3.MoveTowards(tape.transform.position, slots[tape.homeSlot], tapeMoveSpeed);
            print("movin " + timer);
        }
        tape.transform.position = slots[tape.homeSlot];
        activeTape = null;
        screen.PopAll();
    }
}
