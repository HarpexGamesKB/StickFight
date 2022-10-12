using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsFather : MonoBehaviour
{
    public ObjectMover Target;
    public Enemy TargetEnemy;
    public int index;
    void Update()
    {
        if (Target)
        {
            if (!Target.DamageTaken)
            {
                transform.position = Target.transform.position;
                transform.rotation = Target.transform.rotation;
            }
            else
            {
                transform.position = Target.Line.lineRenderer.GetPosition(Target.Line.lineRenderer.positionCount - 1);
            }
        }
        if (TargetEnemy)
        {
            if (!TargetEnemy.DamageTaken)
            {
                transform.position = TargetEnemy.transform.position;
                transform.rotation = TargetEnemy.transform.rotation;
            }
            else
            {
                transform.position = TargetEnemy.Line.lineRenderer.GetPosition(TargetEnemy.Line.lineRenderer.positionCount - 1);
            }
        }
    }
}
