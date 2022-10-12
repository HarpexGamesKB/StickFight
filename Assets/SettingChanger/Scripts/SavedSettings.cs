using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorStruct
{
    public Material material;
    public List<Color> colors = new List<Color>();
    public List<Color> colorsToonCel = new List<Color>();

    public ColorStruct()
    {

    }
    public ColorStruct(ColorStruct shared)
    {
        if (shared.material != null)
        {
            material = new Material(shared.material);
        }
        colors = new List<Color>(shared.colors);
        colorsToonCel = new List<Color>(shared.colorsToonCel);
    }
}

[System.Serializable]
public class SavedSettings : ScriptableObject
{
    public bool useToon;
    public List<ColorStruct> colorStructs;
}
