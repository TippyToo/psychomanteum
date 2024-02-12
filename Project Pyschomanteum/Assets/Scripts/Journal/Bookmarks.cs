using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookmarks : MonoBehaviour
{
    private float maxHeight = 40;
    private float baseHeight;
    // Start is called before the first frame update
    void Start()
    {
        baseHeight = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Lifts bookmark to a desired height
    public void Select() { StartCoroutine("RaiseBookmark"); }
    private IEnumerator RaiseBookmark() {
        float heightChunk = maxHeight / 5;
        for (int i = 0; i < 5; i++) {
            transform.localPosition += new Vector3(0.0f, heightChunk, 0.0f);
            Debug.Log(i);
            yield return new WaitForSecondsRealtime(0.005f); 
        }
    }
    
    public void Deselect() {
        //If the bookmark is no longer selected, place it back to it's original spot
        StopAllCoroutines();
        transform.localPosition = new Vector3(transform.localPosition.x, baseHeight, transform.localPosition.z);
    }
}
