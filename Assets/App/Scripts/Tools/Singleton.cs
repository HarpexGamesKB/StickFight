using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public bool global = true;
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }

    protected void Awake()
    {
        if (global)
        {
            if (instance != null && instance != gameObject.GetComponent<T>())
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = gameObject.GetComponent<T>();
        }
    }
}