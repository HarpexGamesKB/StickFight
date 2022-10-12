using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OSLevelManager))]
public class OSLevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OSLevelManager OSLM = target as OSLevelManager;
        string[] levelList = new string[OSLM.GetLevels().Length];
        for (int i = 0; i < OSLM.GetLevels().Length; i++)
        {
            levelList[i] = OSLM.GetLevels()[i].name;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Level");
        OSLM.LevelIndex = EditorGUILayout.Popup(OSLM.LevelIndex, levelList);
        GUILayout.EndHorizontal();
    }
}