using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnregisterEvents();
    }
    protected virtual void RegisterEvents()
    {

    }
    protected virtual void UnregisterEvents()
    {

    }
}
