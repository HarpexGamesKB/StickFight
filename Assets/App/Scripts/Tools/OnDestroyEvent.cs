using UnityEngine;
using UnityEngine.Events;

public class OnDestroyEvent : MonoBehaviour
{
    public UnityEvent onDestroy;

    private void OnDestroy()
    {
        /*
            if (this.OnDestroyAction != null)
            {
                this.OnDestroyAction();
            }
        */
        onDestroy?.Invoke();
        onDestroy.RemoveAllListeners();
    }

    private void Awake()
    {
        if (onDestroy == null)
            onDestroy = new UnityEvent();
    }

    /*
        public event OnDestroyDelegate OnDestroyAction;

        public delegate void OnDestroyDelegate();
    */
}