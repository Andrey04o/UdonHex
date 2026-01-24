using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
public class RaycastButton : UdonSharpBehaviour
{
    virtual public void OnRaycastEnter() {

    }
    virtual public void OnRaycastExit() {

    }
    virtual public void OnRaycastClick() {

    }
    void OnMouseEnter() {
        OnRaycastEnter();
    }
    void OnMouseExit() {
        OnRaycastExit();
    }
}
