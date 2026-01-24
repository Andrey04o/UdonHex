
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
using UdonSharp;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UdonSharpEditor;
#endif
using Unity.Mathematics;
using UnityEditor;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class HexGame : UdonSharpBehaviour
{
    public Hex[][] hexs;
    public Hex[] hexlist;
    private int height = 11;
    private int width = 16;
    int currentSide = 0;
    public bool isPlayerTakeRed = false;
    public bool isPlayerTakeBlue = false;
    bool isPlayerCanPlace = false;
    public MeshRenderer hexTableIn;
    public MeshRenderer inSelect;
    float timeColor = 2f;
    float timeCurrent = 0f;
    public Shader shader;
    public Material materialChange;
    public Canvas canvasWin;
    public TextMeshProUGUI textWin;
    public HexColorCustomization hexColorCustomization;
    public bool isGameEnd = false;
    public Hex hexPlayerLooking;
    [UdonSynced] public byte[] byteHexNetwork = new byte[31]; // 121 / 8 * 2 = 30.25
    public override void OnDeserialization()
    {
        UnpackToClient(byteHexNetwork);
        if (currentSide == 0) {
            RemovePieces();
        } else {
            if (VRC.SDK3.Network.Stats.TimeInRoom > 5f) PlacePieces();
            else PlacePieces(false);
        }
        ShowIntecrations();
    }
    public void Start() {
        Create2DArray();
        PushFromListTo2D();
        DisableInteractions(true);
        HideWinScreen();
        //hexTableIn.material = new Material();
    }
    void PressRestartUI() {
        
    }

    void Create2DArray() {
        hexs = new Hex[height][];
        for (int i = 0; i < height; i++)
        {
            hexs[i] = new Hex[width];
        }
    }
    void PushFromListTo2D() {
        foreach (Hex hex in hexlist) {
            hexs[(int)hex.position.x][(int)hex.position.y] = hex;
        }
    }
    public void Press(int whichSide = 0) {
        if (hexPlayerLooking == null) return;
        if (hexPlayerLooking.isPlayerLookToThis == false) {
            return;
        }
        Press(hexPlayerLooking, whichSide);
    }
    public void Press(Hex hex, int whichSide = 0) {
        if (currentSide == 0) {
            Press(hex);
            return;
        }
        if (currentSide != whichSide) {
            return;
        }
        Press(hex);
    }

    public void Press(Hex hex) {
        if (currentSide == 0) {
            if (isPlayerTakeRed) {
                currentSide = 1;
            } else {
                currentSide = 2;
            }
        }
        hex.PlacePiece(currentSide);
        currentSide++;
        currentSide = (currentSide % 3);
        if (currentSide == 0) currentSide = 1;
        isPlayerCanPlace = false;
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        ShowIntecrations();
        PackToNetwork();
        RequestSerialization();
    }

    public void ShowIntecrations() {
        DisableInteractions(true);
        if (isGameEnd == true) return;
        if (currentSide == 0) {
            if (isPlayerTakeRed || isPlayerTakeBlue) {
                DisableInteractions(false);
                isPlayerCanPlace = true;
            }
        }
        if (currentSide == 1 && isPlayerTakeRed) {
            DisableInteractions(false);
            isPlayerCanPlace = true;
        }
        if (currentSide == 2 && isPlayerTakeBlue) {
            DisableInteractions(false);
            isPlayerCanPlace = true;
        }
    }

    void DisableInteractions(bool value) {
        inSelect.gameObject.SetActive(!value);
        foreach (Hex hex in hexlist) {
            hex.DisablePieceIntecrations(value);
        }
    }

    public void TakeColor(int side) {
        if (side == 1) {
            isPlayerTakeRed = !isPlayerTakeRed;
        } else {
            isPlayerTakeBlue = !isPlayerTakeBlue;
        }
        ShowIntecrations();
    }
    int countRed = 0;
    int countBlue = 0;
    public void PlayerTakePiece(bool isGrab, int side) {
        if (side == 1) {
            if (isGrab) countRed++;
            else countRed--;
        } else {
            if (isGrab) countBlue++;
            else countBlue--;
        }
        if (countRed > 0) {
            isPlayerTakeRed = true;
        } else {
            isPlayerTakeRed = false;
        }
        if (countBlue > 0) {
            isPlayerTakeBlue = true;
        } else {
            isPlayerTakeBlue = false;
        }
        ShowIntecrations();
    }
    #if !COMPILER_UDONSHARP && UNITY_EDITOR
    public void SetNeightbours() {
        Create2DArray();
        PushFromListTo2D();
        foreach (Hex hex in hexlist) {
            hex.neighbours = HexCreator.GetNeighbours(hex);
            EditorUtility.SetDirty(hex);
        }
    }
#endif
    /*
        public void PackToNetwork() {
            int i = 3;
            int bi = -1;
            foreach (Hex hex in hexlist) {
                if (i > 2) {
                    i = 0;
                    bi++;
                    byteHexNetwork[bi] = 0;
                }
                if (i == 0) {
                    byteHexNetwork[bi] += (byte)(hex.side * 100);
                } else if (i == 1) {
                    byteHexNetwork[bi] += (byte)(hex.side * 10);
                } else {
                    byteHexNetwork[bi] += (byte)(hex.side);
                }
                i++;
            }
        }
        */
    public int lenghtArray = 8;

    public void PackToNetwork() { // Works like 00, 01, 10, 11.  
        int length = hexlist.Length;
        bool[] bits = new bool[byteHexNetwork.Length * 8];
        int gr = 0;
        bool[] bitsG = new bool[8];
        for (int hi = 0; hi < length; hi++) { 
            gr = (int)(hexlist[hi].side / 2);
            bits[hi + gr * length] = (hexlist[hi].side - gr) == 1;
        }
        
        for (int bi = 0; bi < byteHexNetwork.Length; bi++) {
            //Array.Copy(bits, bi * 8, bitsG, 0, lenghtArray);
            CopyArr(bits, bi * 8, bitsG);
            byteHexNetwork[bi] = ConvertBoolArrayToByte(bitsG);
        }

        if (currentSide == 0) {
            bitsG[6] = false;
        } else {
            bitsG[6] = true;
        }

        if (currentSide == 1) {
            bitsG[7] = false;
        } else {
            bitsG[7] = true;
        }
        byteHexNetwork[byteHexNetwork.Length - 1] = ConvertBoolArrayToByte(bitsG);
    }
    private void CopyArr(bool[] arr1, int start, bool[] arr2) {
        for (int i = 0; i < arr2.Length; i++) {
            bool colvar = arr1[start + i];
            arr2[i] = colvar;
        }
    }
    private void CombineArr(bool[] arr1, int start, bool[] arr2) {
        for (int i = 0; i < arr1.Length; i++) {
            arr2[i + start] = arr1[i];
        }
    }

    public void UnpackToClient(byte[] bytes) {
        bool[] bits = new bool[byteHexNetwork.Length * 8];
        bool[] bitsG = new bool[8];

        for (int bi = 0; bi < bytes.Length; bi++) {
            bitsG = ConvertByteToBoolArray(bytes[bi]);
            CombineArr(bitsG, bi * 8, bits);
        }
        /*
        string t = "";
        foreach (bool b in bits) {
            t += " " + b;
        }
        Debug.Log(t);
        */
        if (bitsG[6] == false) {
            currentSide = 0;
            return;
        }
        if (bitsG[7] == false) {
            currentSide = 1;
        } else {
            currentSide = 2;
        }

        int length = hexlist.Length;
        for (int hi = 0; hi < length * 2; hi++) {
            if (hi < length) {
                if (bits[hi] == false) {
                    hexlist[hi].side = 0;
                } else {
                    hexlist[hi].side = 1;
                }
            } else {
                if (bits[hi] == true) {
                    hexlist[hi - length].side = 2;
                }
            }
        }
    }
    public byte[] byteHexNetworkRemote;

    public void PlacePieces(bool sound = true) {
        foreach (Hex hex in hexlist) {
            hex.PlacePiece(hex.side, sound);
        }
    }
    public void RemovePieces() {
        foreach (Hex hex in hexlist) {
            hex.RemovePiece();
        }
        currentSide = 0;
        isGameEnd = false;
        HideWinScreen();
    }
    public void ShowWinScreen(int side) {
        DisableInteractions(true);
        isGameEnd = true;
        canvasWin.gameObject.SetActive(true);
        if (side == 1) {
            textWin.text = "Red win";
            textWin.color = hexColorCustomization.colorRed;
        }
        if (side == 2) {
            textWin.text = "Blue win";
            textWin.color = hexColorCustomization.colorBlue;
        }
    }
    public void HideWinScreen() {
        canvasWin.gameObject.SetActive(false);
    }

//https://stackoverflow.com/questions/24322417/how-to-convert-bool-array-in-one-byte-and-later-convert-back-in-bool-array

    private static byte ConvertBoolArrayToByte(bool[] source)
    {
        byte result = 0;
        // This assumes the array never contains more than 8 elements!
        int index = 8 - source.Length;

        // Loop through the array
        foreach (bool b in source)
        {
            // if the element is 'true' set the bit at that position
            if (b)
                result |= (byte)(1 << (7 - index));

            index++;
        }

        return result;
    }

    private static bool[] ConvertByteToBoolArray(byte b)
    {
        // prepare the return result
        bool[] result = new bool[8];

        // check each bit in the byte. if 1 set to true, if 0 set to false
        for (int i = 0; i < 8; i++)
            result[i] = (b & (1 << i)) != 0;

        // reverse the array
        Array.Reverse(result);

        return result;
    }
}

#if !COMPILER_UDONSHARP && UNITY_EDITOR
[CustomEditor(typeof(HexGame)), CanEditMultipleObjects]
public class CustomInspectorEditor : Editor
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
#endif
