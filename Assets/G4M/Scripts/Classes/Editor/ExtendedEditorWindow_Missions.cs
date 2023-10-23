using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ExtendedEditorWindow_Missions : EditorWindow
{
    protected SerializedObject serialisedObject;
    protected SerializedProperty currentProperty;

    private string selectedPropertyPath;
    protected SerializedProperty selectedProperty;

    protected void DrawProperties(SerializedProperty prop, bool drawChildren)
    {
        string lastPropParth = string.Empty;
        foreach(SerializedProperty p in prop)
        {
            if(p.isArray && p.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                EditorGUILayout.EndHorizontal();

                if (p.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(p, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(lastPropParth) && p.propertyPath.Contains(lastPropParth)) { continue; }
                lastPropParth = p.propertyPath;
                EditorGUILayout.PropertyField(p, drawChildren);
            }
        }
    }

    protected void DrawSidebar(SerializedProperty prop)
    {
        foreach (SerializedProperty p in prop)
        {
            if (GUILayout.Button(p.displayName))
            {
                selectedPropertyPath = p.propertyPath;
            }
        }
        if (!string.IsNullOrEmpty(selectedPropertyPath))
        {
            selectedProperty = serialisedObject.FindProperty(selectedPropertyPath);
        }
    }

    protected void DrawField(string propName, bool relative)
    {
        if(relative && currentProperty != null)
        {

            EditorGUILayout.PropertyField(currentProperty.FindPropertyRelative(propName), true);

            SerializedProperty sp = currentProperty.FindPropertyRelative(propName);

            if (sp.type == "PPtr<$Sprite>")
            {
                    Texture2D texture = AssetPreview.GetAssetPreview(sp.objectReferenceValue);
                    if (texture != null) GUILayout.Label(texture);

            }
            if (sp.type =="vector")
            {
                if (sp.arrayElementType == "PPtr<$Sprite>")
                {
                    for (int i = 0; i < sp.arraySize; i++)
                    {
                        Texture2D texture = AssetPreview.GetAssetPreview(sp.GetArrayElementAtIndex(i).objectReferenceValue);
                        if (texture != null) GUILayout.Label(texture);
                    }
                }
            }


        }
        else if (serialisedObject != null)
        {

            EditorGUILayout.PropertyField(serialisedObject.FindProperty(propName), true);
            SerializedProperty sp = serialisedObject.FindProperty(propName);
            //if (sp.propertyType == SerializedPropertyType.ObjectReference || sp.arrayElementType == "Sprite")
            if (sp.type == "PPtr<$Sprite>")
            {
                Texture2D texture = AssetPreview.GetAssetPreview(sp.objectReferenceValue);
                if (texture != null) GUILayout.Label(texture);

            }
            if (sp.type == "vector")
            {
                if (sp.arrayElementType == "PPtr<$Sprite>")
                {
                    for (int i = 0; i < sp.arraySize; i++)
                    {
                        Texture2D texture = AssetPreview.GetAssetPreview(sp.GetArrayElementAtIndex(i).objectReferenceValue);
                        if (texture != null) GUILayout.Label(texture);
                    }
                }
            }
        }


    }

    protected void Apply()
    {
        serialisedObject.ApplyModifiedProperties();
    }
}
