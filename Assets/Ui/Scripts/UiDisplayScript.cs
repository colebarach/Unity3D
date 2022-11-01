using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  UI Display Script
	
	Author -  Cole Barach
	Created - 2022.09.05
	Updated - 2022.09.06
	
	Function
		-Display of Image to UI

    Dependencies
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class UiDisplayScript : MonoBehaviour {
    [Header("Image")]
    public Texture image;
    public Color   imageColor;
    public float   imageScale;
    [Header("Backdrop")]
    public Color   backdropColorActive;
    public Color   backdropColorPassive;
    [Header("References")]
    public RawImage uiImage;
    public RawImage uiBackdrop;

    void Start() {
        if(uiImage    == null) uiImage    = transform.GetChild(0).GetComponent<RawImage>();
        if(uiBackdrop == null) uiBackdrop = GetComponent<RawImage>();
    }

    void Update() {
        uiImage.texture = image;
        uiImage.color   = imageColor;
        uiImage.rectTransform.sizeDelta = GetScale();
        if(image == null) {
            uiBackdrop.color = backdropColorPassive;
        } else {
            uiBackdrop.color = backdropColorActive;
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