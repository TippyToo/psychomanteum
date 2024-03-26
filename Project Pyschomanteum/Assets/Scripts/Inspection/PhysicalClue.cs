using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicalClue : MonoBehaviour
{
    //Material glowing
    private Renderer myRenderer;
    private List<Material> materials = new();
    private List<Color> emissiveColors = new();
    private bool glow;
    public bool notFound = true;
    public float flickerSpeed;
    public AnimationCurve myCurve;
    public string observation = "";

    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myCurve.postWrapMode = WrapMode.Loop; //Makes sure the flicker is continuous
        //If the object has any imissive materials, add them to the list to be updated
        foreach (Material mat in myRenderer.materials) {
            if (myRenderer.material.enabledKeywords.Any(item => item.name == "_EMISSION") && myRenderer.material.HasColor("_EmissionColor")) {
                materials.Add(mat);
                emissiveColors.Add(mat.GetColor("_EmissionColor"));
            }
        }
        //If the object is being inspected and the clue hasn't been found yet
        if (transform.parent.transform.parent != null && transform.parent.transform.parent.name == "ItemToInspect" &&
            notFound && !transform.parent.GetComponent<ItemBehaviour>().collected) { this.glow = true; }
        else { this.glow = false; gameObject.SetActive(false); }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (glow) { //If the object should be glowing, update the material's imissive intensity based on its animation curve and flicker time;
            float currTime = Time.time * flickerSpeed;
            for (int i = 0; i < materials.Count; i++) {
                float emissiveIntensity = myCurve.Evaluate(currTime);
                Color newColor = emissiveColors[i];
                newColor = new Color(newColor.r * Mathf.Pow(2, emissiveIntensity), newColor.g * Mathf.Pow(2, emissiveIntensity), newColor.b * Mathf.Pow(2, emissiveIntensity), newColor.a);
                materials[i].SetColor("_EmissionColor", newColor);
            }
        }
    }

    public void CollectClue() {
        if (notFound)
        {
            this.notFound = false;
            this.glow = false;
            if (this.GetComponent<Observation>() != null)
            {
                this.GetComponent<Observation>().MakeObservation();
            }
        }
    }
}
