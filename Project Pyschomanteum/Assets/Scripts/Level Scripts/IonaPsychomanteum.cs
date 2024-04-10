using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IonaPsychomanteum : MonoBehaviour
{
    public Dialogue iona;
    public GameObject psychomanteum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iona.conversationToLoad == -1) {
            psychomanteum.GetComponent<SceneTransition>().unlocked = true;
            psychomanteum.GetComponent<IsInteractable>().unlocked = true;
        }
    }
}
