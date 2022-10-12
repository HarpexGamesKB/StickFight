using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailwayIndicator : MonoBehaviour
{
    [SerializeField] private Renderer Renderer;
    [SerializeField] private Material[] Materials;
    [SerializeField] private GameObject[] Shines;
    [SerializeField] private float Delay;
    private Material[] mats;

    public void Enable()
    {
        EnableOne();
    }

    private void EnableOne()
    {
        mats = Renderer.materials;

        mats[2] = Materials[0];
        mats[1] = Materials[1];

        Renderer.materials = mats;
        Shines[0].SetActive(true);
        Shines[1].SetActive(false);
        Invoke(nameof(EnableSecond), Delay);
    }

    private void EnableSecond()
    {
        mats = Renderer.materials;

        mats[1] = Materials[0];
        mats[2] = Materials[1];

        Renderer.materials = mats;
        Shines[1].SetActive(true);
        Shines[0].SetActive(false);
        Invoke(nameof(EnableOne), Delay);
    }

    public void Disable()
    {
        CancelInvoke();
        mats = Renderer.materials;

        mats[2] = Materials[1];
        mats[1] = Materials[1];

        Renderer.materials = mats;
        Shines[0].SetActive(false);
        Shines[1].SetActive(false);
    }
}