using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseButton : MonoBehaviour
{
    [HideInInspector]
    public int toLoad;
    [HideInInspector]
    public GameObject NPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick() {
        NPC.GetComponent<Dialogue>().PlayersResponse(toLoad);
    }
    
}
