using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
namespace Hex04o {
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HexTriggerPlace : UdonSharpBehaviour
{
    public Hex hex;
    HexHeapGrab hexHeapGrab;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        hexHeapGrab = other.GetComponent<HexHeapGrab>();
        if (hexHeapGrab != null) {
            hex.OnRaycastEnter();
            hexHeapGrab.SetHex(hex);
        }
    }
    void OnTriggerExit(Collider other) {
        hexHeapGrab = other.GetComponent<HexHeapGrab>();
        if (hexHeapGrab != null) {
            hex.OnRaycastExit();
            hexHeapGrab.SetHex(null);
        }
    }

}
}
