using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private LayerMask CollisionMask;
    [SerializeField] private Color Color;

    private void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        Material newMaterial = new Material(renderer.material);
        newMaterial.color = Color;
        renderer.material = newMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Tools.HasLayer(CollisionMask, other.gameObject.layer))
        {
        }
    }
}