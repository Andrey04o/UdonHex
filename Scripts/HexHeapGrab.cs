
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace Hex04o {
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HexHeapGrab : UdonSharpBehaviour
{
    public VRC_Pickup pickup;
    public HexGame hexGame;
    public HexHeaps hexHeaps;
    public Animator animator;
    public RaycastButtonHandler raycastButtonHandler;
    public Mesh meshHeap;
    public Mesh meshHex;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public Vector3 positionOriginal;
    public Quaternion rotationOriginal;
    /*[UdonSynced]*/ public bool isGrabbed = false;
    //[UdonSynced] public Vector3 positionSync;
    //[UdonSynced] public Quaternion rotationSync;
    public bool isRed = false;
    public bool isBlue = false;
    Hex hexCurrent;
    void Start() {
        positionOriginal = transform.position;
        rotationOriginal = transform.rotation;
        //positionSync = transform.position;
        //rotationSync = transform.rotation;
    }

    public override void OnDrop()
    {
        base.OnDrop();
        PlaceHeapBack();
    }
    public override void OnPickup()
    {
        base.OnPickup();
        //Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        TakeHex();
    }

    public override void OnPickupUseDown()
    {
        base.OnPickupUseDown();
        if (Networking.LocalPlayer.IsUserInVR())
            return;
        if (isRed)
            hexGame.Press(1);
        if (isBlue)
            hexGame.Press(2);
        pickup.Drop();
    }

    void PlaceHeapBack() {
        isGrabbed = false;
        PlaceHex();
        ChangeFormToHeap();
        //RequestSerialization();
        if (isRed) {
            hexGame.PlayerTakePiece(false, 1);
        }
        if (isBlue) {
            hexGame.PlayerTakePiece(false, 2);
        }
    }

    void TakeHex() {
        isGrabbed = true;
        ChangeFormToHex();
        //RequestSerialization();
        if (isRed) {
            hexGame.PlayerTakePiece(true, 1);
        }
        if (isBlue) {
            hexGame.PlayerTakePiece(true, 2);
        }
    }
    void ChangeFormToHex() {
        meshFilter.mesh = meshHex;
        meshCollider.sharedMesh = meshHex;
        hexHeaps.ShowAnotherHeap();
    }
    void ChangeFormToHeap() {
        meshFilter.mesh = meshHeap;
        meshCollider.sharedMesh = meshHeap;
        transform.position = positionOriginal;
        transform.rotation = rotationOriginal;
        gameObject.SetActive(false);
    }
    public void PlayAnimation() {
        animator.Play("TakeHex", 0, 0f);
    }
    public void SetHex(Hex hex) {
        hexCurrent = hex;
    }
    void PlaceHex() {
        if (hexCurrent != null) {
            if (isRed) {
                hexGame.Press(hexCurrent, 1);
                //hexCurrent.PlacePiece(1);
            }
            if (isBlue) {
                hexGame.Press(hexCurrent, 2);
                //hexCurrent.PlacePiece(2);
            }
        }
    }
    /*
    public override void OnDeserialization() {
        if (isGrabbed == true) {
            ChangeFormToHex();
        } else {
            ChangeFormToHeap();
        }
        transform.rotation = rotationSync;
        transform.position = positionSync;
        //hexHeaps.ShowAnotherHeap();
    }
    void Update() {
        if (isGrabbed == false) return;
        rotationSync = transform.rotation;
        positionSync = transform.position;
    }
    */
}
}