using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class Grabthiss : UdonSharpBehaviour
{
    public VRC_Pickup pickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) {
            //OnPickup();
            //pickup.GenerateHapticEvent(0,0,0);
            //pickup.PlayHaptics();
            //pickup.Drop();
            //VRC_Pickup.HapticEvent.Invoke(pickup, 1f, 1f, 1f);
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }
    public override void OnPickup()
    {
        base.OnPickup();
        Debug.Log("pickuppp");
    }
}
