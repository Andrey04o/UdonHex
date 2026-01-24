using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.Udon.Common;

[DisallowMultipleComponent] [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class CursorController : UdonSharpBehaviour
{
    [Header("References")]
    public Canvas targetCanvas;
    public Image cursorImage;

    [Header("Cursor Settings")]
    public float sensitivity = 2f;
    public Sprite defaultSprite;
    public Vector2 size = new Vector2(32, 32);
    public bool clampToCanvas = true;
    public Vector2 hotspotOffset = Vector2.zero;

    public RectTransform _canvasRect;
    private RectTransform _cursorRect;
    [HideInInspector] public Vector2 currentPosition;
    [HideInInspector] public Vector2 currentPositionScreen;
    [HideInInspector] public Vector2 velocity = Vector2.zero;
    public bool isLocked = false;

    void Start()
    {
        //asd.virtu
        _canvasRect = targetCanvas.GetComponent<RectTransform>();

        _cursorRect = cursorImage.GetComponent<RectTransform>();

        if (defaultSprite != null)
            SetSprite(defaultSprite);
    }
    void Update() {
        velocity.x = Input.GetAxis("Mouse X");
        velocity.y = Input.GetAxis("Mouse Y");
        Move(velocity);
    }
    public void SetSprite(Sprite spr, bool useNativeSize = false)
    {
        cursorImage.sprite = spr;
        if (useNativeSize && spr != null)
        {
            _cursorRect.sizeDelta = new Vector2(spr.rect.width, spr.rect.height);
        }
        else if (size != Vector2.zero)
        {
            _cursorRect.sizeDelta = size;
        }
    }
    
    public void Move(Vector2 delta) {
        if (isLocked == true) return;
        _cursorRect.anchoredPosition = _cursorRect.anchoredPosition + delta * sensitivity;
        ClampCursorToCanvas();
        currentPosition = _cursorRect.anchoredPosition;
        currentPositionScreen = currentPosition + _canvasRect.rect.size * 0.5f;
    }

    public void SetPositionFromScreen(Vector2 screenPosition)
    {
        _cursorRect.anchoredPosition = screenPosition;
        ClampCursorToCanvas();
        currentPosition = _cursorRect.anchoredPosition;
        currentPositionScreen = currentPosition + _canvasRect.rect.size * 0.5f;
    }

    public void ClampCursorToCanvas()
    {
        Vector2 canvasSize = _canvasRect.rect.size;
        Vector2 halfCanvas = canvasSize * 0.5f;

        Vector2 min = -halfCanvas;// + (cursorSize * _cursorRect.pivot);
        Vector2 max = halfCanvas;// - (cursorSize * (Vector2.one - _cursorRect.pivot));

        Vector2 anchored = _cursorRect.anchoredPosition;
        anchored.x = Mathf.Clamp(anchored.x, min.x, max.x);
        anchored.y = Mathf.Clamp(anchored.y, min.y, max.y);

        _cursorRect.anchoredPosition = anchored;
    }
    public override void InputLookVertical(float value, UdonInputEventArgs args)
    {
        base.InputLookVertical(value, args);
        //velocity.y = value;
        
    }

    public override void InputLookHorizontal(float value, UdonInputEventArgs args)
    {
        base.InputLookHorizontal(value, args);
        //velocity.x = value;
    }
    public void SetVisibility(bool value) {
        cursorImage.enabled = value;
    }
}
