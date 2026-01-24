
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace Hex04o {
public class HexHeap : UdonSharpBehaviour
{
    public HexGame hexGame;
    public bool isRed = false;
    public bool isBlue = false;
    public Animator animator;
    void Start()
    {
        
    }
    public void Interact() {
        animator.Play("TakeHex", 0, 0f);
        if (isRed) {
            hexGame.TakeColor(1);
        }
        if (isBlue) {
            hexGame.TakeColor(2);
        }
    }
}

}