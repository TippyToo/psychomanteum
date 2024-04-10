using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    public bool flicker;
    public float flickerSpeed;
    public AnimationCurve myCurve;
    public Renderer temp;
    public Light candle;

    private List<Material> materials = new();
    private List<Color> emissiveColors = new();

    private void Awake()
    {
        temp = temp.GetComponent<Renderer>();
        myCurve.postWrapMode = WrapMode.Loop;
        foreach (Material mat in temp.materials)
        {
            if (temp.material.enabledKeywords.Any(item => item.name == "_EMISSION") && temp.material.HasColor("_EmissionColor"))
            {
                materials.Add(mat);
                emissiveColors.Add(mat.GetColor("_EmissionColor"));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flicker)
        {
            float currTime = Time.time * flickerSpeed;
            //candle.intensity = myCurve.Evaluate(currTime) / 10;
            for (int i = 0; i < materials.Count; i++)
            {
                float emissiveIntensity = myCurve.Evaluate(currTime) * 5;
                Color newColor = emissiveColors[i];
                newColor = new Color(newColor.r * Mathf.Pow(2, emissiveIntensity), newColor.g * Mathf.Pow(2, emissiveIntensity), newColor.b * Mathf.Pow(2, emissiveIntensity), newColor.a);
                materials[i].SetColor("_EmissionColor", newColor);
            }
        }
    }
}
