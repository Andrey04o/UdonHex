#if !COMPILER_UDONSHARP && UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdonSharpEditor;
using UnityEditor;
namespace Hex04o {
    [CustomEditor(typeof(Hex)), CanEditMultipleObjects]
    public class CustomInspectorEditorHex : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            // Update the serialized object to reflect changes
            serializedObject.Update();

            // Draw the default inspector properties (optional)
            DrawDefaultInspector(); 

            // Access the target object (the CustomInspectorBehaviour instance)
            Hex myTarget = (Hex)target;

            // Example of custom UI elements
            EditorGUILayout.Space();

            if(GUILayout.Button("Find neighbours")) {
                myTarget.neighbours = HexCreator.GetNeighbours(myTarget);
            }

            // Apply modified properties back to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
