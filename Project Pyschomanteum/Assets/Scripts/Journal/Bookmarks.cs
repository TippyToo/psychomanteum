using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookmarks : MonoBehaviour
{
    private float maxHeight = 40;
    private float baseHeight;
    private InventoryManager inventory;
    private GameObject presentButton;
    // Start is called before the first frame update
    void Start()
    {
        presentButton = FindObjectOfType<JournalManager>().transform.GetChild(6).gameObject;
        inventory = FindObjectOfType<InventoryManager>();
        baseHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateInventory() {
        //inventory.UpdateInventory();
        presentButton.SetActive(false);
    }

    //Lifts bookmark to a desired height
    public void Select() { StartCoroutine("RaiseBookmark"); }
    private IEnumerator RaiseBookmark() {
        float heightChunk = maxHeight / 5;
        for (int i = 0; i < 5; i++) {
            transform.localPosition += new Vector3(0.0f, heightChunk, 0.0f);
            yield return new WaitForSecondsRealtime(0.005f); 
        }
    }
    
    public void Deselect() {
        //If the bookmark is no longer selected, place it back to it's original spot
        StopAllCoroutines();
        transform.localPosition = new Vector3(transform.localPosition.x, baseHeight, transform.localPosition.z);
    }
}
