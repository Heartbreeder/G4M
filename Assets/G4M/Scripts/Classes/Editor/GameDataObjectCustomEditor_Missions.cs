using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler_Missions
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceid, int line)
    {
        MissionArray obj = EditorUtility.InstanceIDToObject(instanceid) as MissionArray;
        if (obj != null)
        {
            GameDataObjectEditorWindow_Missions.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(MissionArray))]
public class GameDataObjectCustomEditor_Missions : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Open Editor"))
            GameDataObjectEditorWindow_Missions.Open((MissionArray) target);
    }
}
