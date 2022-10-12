using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Detector : MonoBehaviour
{
    public event UnityAction<Transform> OnDetect;

    public event UnityAction<Transform> OnUndetect;

    [SerializeField] private LayerMask LayerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (Tools.HasLayer(LayerMask, other.gameObject.layer))
        {
            OnDetect?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Tools.HasLayer(LayerMask, other.gameObject.layer))
        {
            OnUndetect?.Invoke(other.transform);
        }
    }
}