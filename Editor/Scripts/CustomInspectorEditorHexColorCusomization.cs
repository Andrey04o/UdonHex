#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UdonSharpEditor;
using UnityEditor.SceneManagement;
namespace Hex04o {
    [CustomEditor(typeof(HexColorCustomization)), CanEditMultipleObjects]
    public class CustomInspectorEditorHexColorCusomization : Editor
    {
        public override void OnInspectorGUI()
        {
            if (UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target)) return;
            // Update the serialized object to reflect changes
            serializedObject.Update();

            // Draw the default inspector properties (optional)
            DrawDefaultInspector(); 

            // Access the target object (the CustomInspectorBehaviour instance)
            HexColorCustomization myTarget = (HexColorCustomization)target;

            // Example of custom UI elements
            EditorGUILayout.Space();
            if(GUILayout.Button("Default")) {
                myTarget.colorRed = new Color(255, 112, 112, 255);
                myTarget.colorBlue = new Color(112, 215, 255, 255);
                myTarget.colorLines = new Color(255, 255, 255, 255);
                myTarget.colorBackground = new Color(200, 200, 200, 255);
                myTarget.SetMaterialsDefault();
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
            }
            if(GUILayout.Button("SetColors")) {
                myTarget.SetColors();
                EditorUtility.SetDirty(myTarget);
                EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
            }
            EditorGUILayout.LabelField("Check color theme");
            EditorGUI.BeginChangeCheck();
            HexColorTheme hexColorTheme = null;
            hexColorTheme = (HexColorTheme)EditorGUILayout.ObjectField(hexColorTheme, typeof(HexColorTheme), true);
            /*
            if(GUILayout.Button("Check color theme")) {
                if (hexColorTheme != null) {
                    myTarget.SetColors();
                    EditorUtility.SetDirty(myTarget);
                    EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
                }
                
            }
            */
            
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(myTarget, "Change theme");
                myTarget.SetColors(hexColorTheme);
            }

            // Apply modified properties back to the serialized object
            serializedObject.ApplyModifiedProperties();
        }
    }
    
}
#endif
