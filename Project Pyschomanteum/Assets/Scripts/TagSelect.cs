using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSelect : MonoBehaviour
{

    private Transform self;
    private Vector3 displacement;
    public float selectHeight;
    // Start is called before the first frame update
    void Start()
    {
        self = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator VertTabHover() {
        for (int i = 0; i < 10; i++) { 
            yield return new WaitForSeconds(0.005f);
            displacement += new Vector3(0.0f, selectHeight, 0.0f);
            self.GetChild(1).localPosition += new Vector3(0.0f, selectHeight, 0.0f);
            self.GetChild(0).localPosition += new Vector3(0.0f, selectHeight, 0.0f);
        }
    }
    public void Select()
    {
        StartCoroutine("VertTabHover");
    }
    public void Deselect() {
        self.GetChild(0).localPosition -= displacement;
        self.GetChild(1).localPosition -= displacement;
        displacement = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
