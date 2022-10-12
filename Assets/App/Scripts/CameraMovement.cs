using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform target2;
    [SerializeField] private float MoveSpeed =2;
    [SerializeField] private float RotationSpeed =2;
    public bool CanMove;
    public bool MoveToTarget2;


    private void Update()
    {
        if (!CanMove) return;
        if (MoveToTarget2)
        {
            MoveSpeed = 6;
            RotationSpeed = 6;
            transform.position = Vector3.Lerp(transform.position, target2.position, Time.deltaTime * MoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target2.rotation, Time.deltaTime * RotationSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * MoveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Time.deltaTime * RotationSpeed);
        }
    }
}