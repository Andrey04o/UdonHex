using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexUI : MonoBehaviour
{
    [Range(0f, 1f)] //1
    public float alphaLevel = 1f;
    Button button;
    Image image;
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = alphaLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
