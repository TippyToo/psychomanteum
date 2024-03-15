using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerbalClueData
{
    public string name;
    public string issuer;
    public string description;
    public int chapter;

    public VerbalClueData(string name, string issuer, string description, int chapter)
    {
        this.name = name;
        this.issuer = issuer;
        this.description = description;
        this.chapter = chapter;
    }
    public VerbalClueData() { 
        name = "";
        description = "";
        chapter = 0;
    }
}
