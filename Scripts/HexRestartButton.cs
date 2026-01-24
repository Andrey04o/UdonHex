
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class HexRestartButton : UdonSharpBehaviour
{
    public HexGame hexGame;
    public Button button;
    public Animator animatorRestart;
    public TextMeshProUGUI textRestart;

    public void Press() {
        if (hexGame.isGameEnd) {
            Restart();
            return;
        }
        if (textRestart.gameObject.activeSelf == false) {
            animatorRestart.Play("RestartButton", 0, 0f);
        } else {
            Restart();
        }
    }

    public void Restart() {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
        hexGame.RemovePieces();
        hexGame.PackToNetwork();
        hexGame.RequestSerialization();
        hexGame.ShowIntecrations();
    }

    public void Resync() {
        Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            hexGame.RemovePieces();
            hexGame.UnpackToClient(hexGame.byteHexNetwork);
            hexGame.PlacePieces(false);
            hexGame.PackToNetwork();
            hexGame.RequestSerialization();
    }
}
