
using UdonSharp;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UdonSharpEditor;
#endif
using UnityEditor;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HexColorCustomization : UdonSharpBehaviour
{
    public Color colorRed;
    public Color colorBlue;
    public Color colorLines;
    public Color colorBackground;
    public MeshRenderer In;
    public MeshRenderer InSelect;
    public MeshRenderer side1; // blue
    public MeshRenderer side2; // red
    public MeshRenderer grid;
    public MeshRenderer[] hexHeapRed;
    public MeshRenderer[] hexHeapBlue;
    [Header("Default Materials")]
    public Material materialHex;
    public Material materialRedDefault;
    public Material materialBlueDefault;
    public Material materialSide1Default;
    public Material materialSide2Default;
    public Material materialInDefault;
    public Material materialGridDefault;
    [Header("Instanced Materials")]
    public Material materialRed;
    public Material materialBlue;
    public Material materialSide1;
    public Material materialSide2;
    public Material materialIn;
    public Material materialGrid;
    public MaterialPropertyBlock materialPropertyBlock;
    public Hex[] hexs;
    void Start() {
        //SetMaterials();
    }
    public void SetColors() {
        #if !COMPILER_UDONSHARP && UNITY_EDITOR
        materialSide1 = Instantiate(materialSide1Default);
        materialSide2 = Instantiate(materialSide2Default);
        materialIn = Instantiate(materialInDefault);
        materialGrid = Instantiate(materialGridDefault);
        materialBlue = Instantiate(materialHex);
        materialRed = Instantiate(materialHex);
        SetMaterials();
        #endif
        materialSide1.color = colorBlue;
        materialSide2.color = colorRed;
        materialIn.color = colorBackground;
        materialGrid.color = colorLines;
        materialRed.color = colorRed;
        materialBlue.color = colorBlue;
    }
    public void SetMaterials() {
        foreach (MeshRenderer meshRenderer in hexHeapRed) {
            meshRenderer.material = materialRed;
        }
        foreach (MeshRenderer meshRenderer in hexHeapBlue) {
            meshRenderer.material = materialBlue;
        }
        In.material = materialIn;
        side1.material = materialSide1;
        side2.material = materialSide2;
        grid.material = materialGrid;
        foreach (Hex hex in hexs) {
            hex.materialBlue = materialBlue;
            hex.materialRed = materialRed;
        }
    }
    public void SetMaterialsDefault() {
        foreach (MeshRenderer meshRenderer in hexHeapRed) {
            meshRenderer.material = materialRedDefault;
        }
        foreach (MeshRenderer meshRenderer in hexHeapBlue) {
            meshRenderer.material = materialBlueDefault;
        }
        In.material = materialInDefault;
        side1.material = materialSide1Default;
        side2.material = materialSide2Default;
        grid.material = materialGridDefault;
        foreach (Hex hex in hexs) {
            hex.materialBlue = materialBlueDefault;
            hex.materialRed = materialRedDefault;
        }
    }
    public void SetColors(HexColorTheme hct) {
        colorBlue = hct.colorBlue;
        colorRed = hct.colorRed;
        colorBackground = hct.colorBackground;
        colorLines = hct.colorLines;
        SetColors();
    }
    void SetColor(MeshRenderer meshRenderer, Color color) {
        //MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        //materialPropertyBlock.SetColor("_Color", color);
        //meshRenderer.SetPropertyBlock(materialPropertyBlock);
    }
}

#if !COMPILER_UDONSHARP && UNITY_EDITOR
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
#endif
