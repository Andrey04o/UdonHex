using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common;
namespace Hex04o {
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class RaycastButtonHandler : UdonSharpBehaviour
{
    public Camera mainCamera;
    public CursorController cursorController;
    public LayerMask tileLayerMask = -1; // Default to all layers
    
    private RaycastButton _currentHoveredTile;
    private RaycastButton _lastHoveredTile;

    private void Update()
    {
        HandleTileRaycast();
    }

    public override void InputUse(bool value, UdonInputEventArgs args)
    {
        if (value == true) {
            //Press();
        }
    }

    private void HandleTileRaycast()
    {
        //Networking.LocalPlayer
        //Ray ray = mainCamera.ViewportPointToRay(cursorController.currentPositionScreen);
        #if UNITY_2022_3_62
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        #else
        //Ray ray = mainCamera.ScreenPointToRay(cursorController.currentPositionScreen);
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        #endif
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
        {
            RaycastButton tile = hit.collider.GetComponent<RaycastButton>();
            
            if (tile != null)
            {
                
                _currentHoveredTile = tile;

                // Handle hover enter
                if (_lastHoveredTile != tile)
                {
                    // Exit previous tile
                    if (_lastHoveredTile != null)
                    {
                        _lastHoveredTile.OnRaycastExit();
                    }

                    // Enter new tile
                    tile.OnRaycastEnter();
                    _lastHoveredTile = tile;
                }

                // Handle click
                if (Input.GetMouseButtonDown(0))
                {
                    tile.OnRaycastClick();
                }
            }
            else
            {
                // Hit something but not a tile
                HandleNoTileHit();
            }
        }
        else
        {
            // Didn't hit anything
            HandleNoTileHit();
        }
    }

    private void HandleNoTileHit()
    {
        if (_lastHoveredTile != null)
        {
            _lastHoveredTile.OnRaycastExit();
            _lastHoveredTile = null;
        }
        _currentHoveredTile = null;
    }
    public void Activate() {
        if (Networking.LocalPlayer.IsUserInVR() == true) return;
        //mainCamera = Camera.main;
        gameObject.SetActive(true);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
}