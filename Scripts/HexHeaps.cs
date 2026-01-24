
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace Hex04o {
public class HexHeaps : UdonSharpBehaviour
{
    public HexHeapGrab[] list;
    void Start()
    {
        
    }
    public void ShowAnotherHeap() {
        foreach(HexHeapGrab hexHeapGrab in list) {
            if (hexHeapGrab.isGrabbed == false) {
                hexHeapGrab.gameObject.SetActive(true);
                hexHeapGrab.PlayAnimation();
                break;
            }
        }
    }
}
}
