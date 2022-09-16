using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UiDisplayScript : MonoBehaviour {
    [Header("Image")]
    public Texture image;
    public Color imageColor;
    public float imageScale;
    [Header("Container")]
    public Color containerColor;
    
    RawImage imageObject;
    RawImage container;

    void Start() {
        imageObject = transform.GetChild(0).GetComponent<RawImage>();
        container = GetComponent<RawImage>();
    }

    void Update() {
        imageObject.texture = image;
        imageObject.color = imageColor;
        imageObject.rectTransform.sizeDelta = GetScale();
        if(image == null) {
            container.color = Color.clear;
        } else {
            container.color = containerColor;
        }
    }

    Vector2 GetScale() {
        Vector2 scale = new Vector2(1,1);
        if(image != null) {
            scale.x = image.width;
            scale.y = image.height;
        }
        scale *= imageScale;
        return scale;
    }
}