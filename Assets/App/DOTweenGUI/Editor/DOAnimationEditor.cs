using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [CustomEditor(typeof(DOAnimation))]
public class DOAnimationEditor : Editor
{
    DOAnimation doAnimation;
    private void OnEnable()
    {
        doAnimation = target as DOAnimation;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Animate"))
        {
            doAnimation.Animate();
        }
    }
}
