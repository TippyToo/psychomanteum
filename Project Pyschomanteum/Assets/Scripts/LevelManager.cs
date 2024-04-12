using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int level;
    public bool usingStarterCoords = false;
    public Vector3 playerStartingCoords;
    // Start is called before the first frame update
    void Start()
    {
        if (usingStarterCoords && FindObjectOfType<SaveManager>().preSubWorldCoords.Count > 0) GameObject.Find("Player").GetComponent<PlayerController>().transform.position = playerStartingCoords;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
