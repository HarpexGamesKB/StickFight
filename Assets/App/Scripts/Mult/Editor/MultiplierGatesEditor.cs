using UnityEditor;

[CustomEditor(typeof(MultiplierGates))]
public class MultiplierGatesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MultiplierGates MG = target as MultiplierGates;
    }
}