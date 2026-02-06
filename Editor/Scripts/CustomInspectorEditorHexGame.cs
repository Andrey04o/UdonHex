#if !COMPILER_UDONSHARP && UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonSharpEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Hex04o {
[CustomEditor(typeof(HexGame)), CanEditMultipleObjects]
public class CustomInspectorEditorHexGame : Editor
{
    public override void OnInspectorGUI()
    {
        if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
        // Update the serialized object to reflect changes
        serializedObject.Update();

        // Draw the default inspector properties (optional)
        DrawDefaultInspector(); 

        // Access the target object (the CustomInspectorBehaviour instance)
        HexGame myTarget = (HexGame)target;

        // Example of custom UI elements
        EditorGUILayout.Space();
        if(GUILayout.Button("Set neightbours")) {
            myTarget.SetNeightbours();
            EditorUtility.SetDirty(myTarget);
            EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
        }

        if (GUILayout.Button("FindShader")) {
            myTarget.shader = Shader.Find("Hidden/VRChat/MobileHighlight");
        }

        if (GUILayout.Button("SetShader")) {
            myTarget.materialChange.shader = myTarget.shader;
        }

        if (GUILayout.Button("Pack network")) {
            myTarget.PackToNetwork();
        }
        if (GUILayout.Button("Remove pieces")) {
            myTarget.byteHexNetworkRemote = myTarget.byteHexNetwork;
            myTarget.RemovePieces();
        }
        if (GUILayout.Button("Unpack network")) {
            myTarget.UnpackToClient(myTarget.byteHexNetworkRemote);
            myTarget.PlacePieces();
        }

        // Apply modified properties back to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
    
}
}
#endif
