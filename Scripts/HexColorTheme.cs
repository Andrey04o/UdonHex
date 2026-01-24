
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace Hex04o {
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HexColorTheme : UdonSharpBehaviour
{
    public Color colorRed = new Color(255, 112, 112, 255);
    public Color colorBlue = new Color(112, 215, 255, 255);
    public Color colorLines = new Color(255, 255, 255, 255);
    public Color colorBackground = new Color(200, 200, 200, 255);
}
}