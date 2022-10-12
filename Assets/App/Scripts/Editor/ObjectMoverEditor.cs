using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectMover))]
public class ObjectMoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ObjectMover objectMover = target as ObjectMover;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Ray"))
        {
            objectMover.SetRay();
        }
        GUILayout.Space(10);

        if (GUILayout.Button("Def"))
        {
            objectMover.Default();
        }
    }
}