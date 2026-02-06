#if !COMPILER_UDONSHARP && UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UdonSharpEditor;
using UnityEditor.SceneManagement;
namespace Hex04o {
    [CustomEditor(typeof(HexGrid)), CanEditMultipleObjects]
    public class CustomInspectorEditorHexGrid : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            // Update the serialized object to reflect changes
            serializedObject.Update();

            // Draw the default inspector properties (optional)
            DrawDefaultInspector(); 

            // Access the target object (the CustomInspectorBehaviour instance)
            HexGrid myTarget = (HexGrid)target;

            // Example of custom UI elements
            EditorGUILayout.Space();
            if(GUILayout.Button("Create")) {
                myTarget.Create();
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
            }
            if(GUILayout.Button("PushCellsToHexGame")) {
                myTarget.PushCellsToHexGame();
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
            }

            // Apply modified properties back to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif