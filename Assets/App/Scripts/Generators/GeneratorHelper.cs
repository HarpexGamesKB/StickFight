using UnityEngine;

public class GeneratorHelper : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Matrix4x4 matrix = transform.GetChild(i).localToWorldMatrix;
            Gizmos.matrix = matrix;
            Gizmos.color = Color.black;
            Gizmos.DrawCube(Vector3.zero + Vector3.forward * 5f / 2, new Vector3(.2f, .2f, 5f));
            Gizmos.DrawCube(Vector3.zero, new Vector3(.8f, .3f, .3f));
        }
    }
}