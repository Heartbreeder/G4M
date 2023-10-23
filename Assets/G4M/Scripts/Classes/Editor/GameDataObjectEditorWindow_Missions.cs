using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameDataObjectEditorWindow_Missions : ExtendedEditorWindow_Missions
{
    public static void Open(MissionArray dataObject)
    {
        GameDataObjectEditorWindow_Missions window = GetWindow<GameDataObjectEditorWindow_Missions>("Mission Data Editor");
        window.serialisedObject = new SerializedObject(dataObject);
    }

    private void OnGUI()
    {
        currentProperty = serialisedObject.FindProperty("templateArray");
        //DrawProperties(currentProperty, true);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSidebar(currentProperty);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if(selectedProperty != null)
        {
            //DrawProperties(selectedProperty, true);
            DrawSelectedPropertiesPanel();
        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list.");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        Apply();
    }

    int selection = 0;
    void DrawSelectedPropertiesPanel()
    {
        currentProperty = selectedProperty;

        EditorGUILayout.BeginHorizontal("box");

        if (GUILayout.Button("Basic Data", EditorStyles.toolbarButton))
        {
            selection = 1;
        }

        if (GUILayout.Button("Mission Description", EditorStyles.toolbarButton))
        {
            selection = 2;
        }

        if (GUILayout.Button("Mission Code", EditorStyles.toolbarButton))
        {
            selection = 3;
        }

        if (GUILayout.Button("Mission Solution", EditorStyles.toolbarButton))
        {
            selection = 4;
        }

        EditorGUILayout.EndHorizontal();

        if (selection == 1)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("Name", true);
            DrawField("ID", true);
            DrawField("Stage", true);
            DrawField("isThisMissionTutorialForTheStage", true);
            EditorGUILayout.EndVertical();
        }

        if (selection == 2)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("Description", true);
            DrawField("MissionDetails", true);
            //DrawField("RequestedToolID", true);
            //DrawField("RequestedToolDimensions", true);
            DrawField("FinishedModel", true);

            //EditorGUILayout.BeginHorizontal("box");
            /*
            EditorGUILayout.BeginVertical("box");
            DrawField("CarvingTool", true);
            EditorGUILayout.EndVertical();
            */
            /*
            EditorGUILayout.BeginVertical("box");
            DrawField("FinalProduct", true);
            EditorGUILayout.EndVertical();
            */
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.BeginVertical("box");
            DrawField("CarvingPathBlueprint", true);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box");
            DrawField("MechanicalBlueprint", true);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal("box");
            DrawField("RequestedToolList",true);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }

        if (selection == 3)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("TopCode", true);
            DrawField("MiddleCode", true);
            DrawField("BottomCode", true);
            DrawField("TopCodeDescription", true);
            DrawField("MiddleCodeDescription", true);
            DrawField("BottomCodeDescription", true);

            EditorGUILayout.EndVertical();
        }

        if (selection == 4)
        {
            EditorGUILayout.BeginVertical("box");
            DrawField("CarvingVideo", true);
            DrawField("CarvingSnapshots", true);
            EditorGUILayout.EndVertical();
        }

    }
}
