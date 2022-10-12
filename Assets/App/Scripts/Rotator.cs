using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float Speed;

    private void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * Speed);
    }
}