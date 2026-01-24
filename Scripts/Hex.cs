
using TMPro;
using UdonSharp;
#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UdonSharpEditor;
#endif
using UnityEditor;
using UnityEngine;
using VRC.SDK3.UdonNetworkCalling;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class Hex : RaycastButton
{
    public Vector2 position;
    public TextMeshProUGUI text;
    public HexGame hexGame;
    public MeshRenderer hexObject;
    public MeshRenderer hexObjectGhost;
    public Material materialWhite;
    public Material materialRed;
    public Material materialBlue;
    public Hex[] neighbours;
    public AudioSource audioSource;
    public bool isPlaced = false;
    public bool isEndBlue1 = false;
    public bool isEndBlue2 = false;
    public bool isEndRed1 = false;
    public bool isEndRed2 = false;
    public int side = 0;
    Hex[] neighboursColor = new Hex[6];
    bool isHasEnd1 = false;
    bool isHasEnd2 = false;
    public int weight = 0;
    bool isInteractive = false;
    public bool isPlayerLookToThis = false;
    public HexTriggerPlace hexTriggerPlace;
    public void DisablePieceIntecrations(bool value) {
        if (!isPlaced) {
            SetInteraction(!value);
        }
    }
    public void SetInteraction(bool value) {
        DisableInteractive = !value;
        isInteractive = value;
        hexTriggerPlace.gameObject.SetActive(isInteractive);
        if (isInteractive == false) {
            HideGhost();
        }
    }
    public void PlacePiece(int side, bool sound = true) {
        //if (side == this.side) return;
        //side = side + 1;
        //if (side == 0) return;
        if (side == 0 && isPlaced) {
            RemovePiece();
            return;
        }
        if (side == 0) {
            return;
        }
        this.side = side;
        hexObject.gameObject.SetActive(true);
        if (side == 1) {
            hexObject.material = materialRed;
        }
        if (side == 2) {
            hexObject.material = materialBlue;
        }
        SetInteraction(false);
        audioSource.gameObject.SetActive(true);
        if (isPlaced == false) if (sound == true) audioSource.Play();
        isPlaced = true;
        GetValue();
    }
    public void RemovePiece() {
        hexObject.material = materialWhite;
        hexObject.gameObject.SetActive(false);
        audioSource.gameObject.SetActive(false);
        isPlaced = false;
        side = 0;
        weight = 0;
    }

    void GetValue() {
        if (isEndBlue1 && side == 2) {
            weight = 1;
        }
        if (isEndBlue2 && side == 2) {
            weight = -1;
        }
        if (isEndRed1 && side == 1) {
            weight = 1;
        }
        if (isEndRed2 && side == 1) {
            weight = -1;
        }
        foreach (Hex hex in neighbours) {
            if (hex.side == side) {
                if (hex.weight == 0) {
                    continue;
                } else {
                    if (CheckIfItHaveEnd(hex)) {
                        hexGame.ShowWinScreen(side);
                        return;
                    }
                }
                if (hex.weight > 0) {
                    if (weight > hex.weight || weight == 0) {
                        weight = hex.weight + 1;
                    }
                }
                if (hex.weight < 0) {
                    if (weight < hex.weight || weight == 0) {
                        weight = hex.weight - 1;
                    }
                }
            }
        }
        text.text = "" + weight;
        SetValueOther();
    }

    void SetValueOther() {
        if (weight == 0) {
            return;
        }
        foreach(Hex hex in neighbours) {
            if (hex.side == side) {
                if (hex.weight == 0) {
                    if (weight > 0) hex.weight = weight + 1;
                    if (weight < 0) hex.weight = weight - 1;
                    hex.SetValueOther();
                    hex.text.text = "" + hex.weight;
                }
                continue;
            }
        }
    }
    bool CheckIfItHaveEnd(Hex hex) {
        if (weight > 0) {
            if (hex.weight < 0) {
                return true;
            }
        }
        if (weight < 0) {
            if (hex.weight > 0) {
                return true;
            }
        }
        return false;
    }

    void CheckMyNeighbours() {
        /*
        foreach (Hex hex in neighbours) {
            if (hex.side == side) {
                neighboursColor.AddItem<Hex>(hex);
                if (!hex.neighboursColor.Contains(hex)) {
                    hex.neighboursColor.AddItem(hex);
                }
                
            }
        }
        */
    }
    public override void OnRaycastClick()
    {
        base.OnRaycastClick();
        if (!isInteractive) return;
        hexGame.Press(this);
        HideGhost();
    }
    public override void OnRaycastEnter()
    {
        base.OnRaycastEnter();
        if (!isInteractive) return;
        hexGame.hexPlayerLooking = this;
        ShowGhost();
    }
    public override void OnRaycastExit()
    {
        base.OnRaycastExit();
        if (!isInteractive) return;
        HideGhost();
    }
    void ShowGhost() {
        hexObjectGhost.gameObject.SetActive(true);
        isPlayerLookToThis = true;
    }
    void HideGhost() {
        hexObjectGhost.gameObject.SetActive(false);
        isPlayerLookToThis = false;
    }
    void Start()
    {
        
    }

    void Update()
    {
    }

}

#if !COMPILER_UDONSHARP && UNITY_EDITOR
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
#endif
