using UnityEngine;

public class GeneratorField : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float MinAngle;

    [Range(0f, 1f)]
    [SerializeField] private float MaxAngle;

    [SerializeField] private float Height;
    [SerializeField] private float Width;

    [SerializeField] private bool Debug;

    [Range(0.01f, 50f)]
    [SerializeField] private float VisualLength = 0.01f;

    [Range(0.01f, 1f)]
    [SerializeField] private float VisualDensity = 0.01f;

    public Vector3 TopPosition => transform.position + Vector3.forward * Height;

    public Vector3 BottomPosition => transform.position + Vector3.right * Width;

    public Vector3 GetRandomDirection()
    {
        float min = Mathf.Min(MinAngle, MaxAngle);
        float max = Mathf.Max(MinAngle, MaxAngle);

        float randomAngle = Random.Range(min, max);
        return new Vector3(Mathf.Sin(randomAngle * 2 * Mathf.PI), 0, Mathf.Cos(randomAngle * 2 * Mathf.PI));
    }

    public Vector3 RandomPosition => Tools.RandomV3(BottomPosition, TopPosition);
    public Vector3 RandomDirection => GetRandomDirection();

    private void OnDrawGizmos()
    {
        if (!Debug) return;
        Gizmos.color = Color.black;
        Vector3 directionMin = new Vector3(Mathf.Sin(MinAngle * 2 * Mathf.PI), 0, Mathf.Cos(MinAngle * 2 * Mathf.PI)) * VisualLength;
        Vector3 directionMax = new Vector3(Mathf.Sin(MaxAngle * 2 * Mathf.PI), 0, Mathf.Cos(MaxAngle * 2 * Mathf.PI)) * VisualLength;
        for (float i = 0; i < 1; i += VisualDensity / VisualLength)
        {
            Gizmos.DrawLine(transform.position + directionMax * i + Vector3.right * Width, transform.position + directionMin * i + Vector3.forward * Height);
        }
        Gizmos.DrawRay(transform.position + Vector3.forward * Height, directionMin);
        Gizmos.DrawRay(transform.position + Vector3.right * Width, directionMax);
    }
}