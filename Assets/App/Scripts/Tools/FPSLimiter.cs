using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int _maxFPS;

    private void Awake()
    {
        Application.targetFrameRate = _maxFPS;
    }
}
