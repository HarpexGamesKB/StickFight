using UnityEngine;

public class Rig : MonoBehaviour
{
    private Rigidbody[] _rigidbodies;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = true;
        }
    }

    public void Enable()
    {
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].isKinematic = false;
        }
        _animator.enabled = false;
    }

    public void Push(Vector3 force)
    {
        transform.parent = null;
        Enable();
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _rigidbodies[i].AddForce(force, ForceMode.Impulse);
        }
    }
}