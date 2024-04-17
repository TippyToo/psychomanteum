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
        tape.transform.position = slots[0];
        activeTape = tape;
        screen.Push(tape.associatedScreen);
    }

    void DeactivateTape(Tape tape) 
    {
        tape.transform.position = slots[tape.homeSlot];
        screen.PopAll();
        activeTape = null;
        
    }
}
