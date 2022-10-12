using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollection : MonoBehaviour
{

    public List<LineRenderer> LineList;
    
    
    public LineRenderer GetRandomLineRenderer()
    {
        LineRenderer randomRenderer = LineList[Random.Range(0, LineList.Count)];
        return randomRenderer;
    }
}
