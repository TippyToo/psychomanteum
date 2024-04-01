using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warp : MonoBehaviour
{
    public bool interactable = true;
    public Vector3 warpLocation = new Vector3();
    [TextArea(3, 50)]
    public string warpConfirmationText = "";
    private GameObject player;
    bool detectsPlayer = false;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    void Update()
    {
        if (detectsPlayer && Input.GetButtonUp("Interact"))
        {
            //Bring up confirm warp menu and populate appropriately
            GameObject temp = GameObject.Find("UI").transform.GetChild(4).gameObject;
            if (warpConfirmationText != "") { temp.transform.GetChild(0).GetComponent<Text>().text = warpConfirmationText; }
            else if (gameObject.name == "Warp Confirmation") { temp.transform.GetChild(0).GetComponent<Text>().text = "Warp?"; }
            temp.SetActive(true);
            temp.GetComponent<Warp>().warpLocation = warpLocation;
        }
    }
    public void Teleport() {
        player.transform.position = warpLocation;
        if (gameObject.name == "Warp Confirmation") {
            Cancel();
        }
    }
    public void Cancel() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            detectsPlayer = true;
            if (!interactable)
            {
                Teleport();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            detectsPlayer = false;
        }
    }
}
