
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
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
        public bool isGrabbed = false;
        public bool isRed = false;
        public bool isBlue = false;
        Hex hexCurrent;
        void Start() {
            positionOriginal = transform.position;
            rotationOriginal = transform.rotation;
        }

        public override void OnDrop()
        {
            base.OnDrop();
            PlaceHeapBack();
        }
        public override void OnPickup()
        {
            base.OnPickup();
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
                }
                if (isBlue) {
                    hexGame.Press(hexCurrent, 2);
                }
            }
        }
    }
}