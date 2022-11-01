using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiMenuScript : MonoBehaviour {
    [Header("Menu")]
    public bool                 isOpen;
    public float                volume;
    [Header("Input")]
    public KeyCode              openKey;
    public KeyCode              cursorUpKey;
    public KeyCode              cursorDownKey;
    public KeyCode              increaseSelected;
    public KeyCode              decreaseSelected;
    public KeyCode[]            closeOverrides;
    [Header("Style")]
    public int                  fontSize;
    public TextAnchor           alignment;
    [Header("References")]
    public UiTextScript         uiText;
    public int                  channel;

    void Update() {
        GetInput();
        if(isOpen) Render();
    }
    void GetInput() {
        if(Input.GetKeyDown(openKey)) {
            if(isOpen) {
                Close();
            } else {
                Open();
            }
        }
        for(int x = 0; x < closeOverrides.Length; x++) {
            if(Input.GetKeyDown(closeOverrides[x])) {
                Close();
            }
        }
        if(!isOpen) return;
        if(Input.GetKeyDown(cursorUpKey)) {
            uiText.MoveCursor(-1, channel);
        }
        if(Input.GetKeyDown(cursorDownKey)) {
            uiText.MoveCursor(1,  channel);
        }
    }
    void Render() {
        uiText.StartQueue(                          channel);
        uiText.QueueNoCursor(                       channel);
        uiText.QueueText(       "Menu:",            channel);
        uiText.QueueLineEnd(                        channel);

        uiText.QueueText(       " ",                channel);
        uiText.QueueCursor(                         channel);
        uiText.QueueText(       " Volume: ",        channel);
        uiText.QueueBar(        volume, 16,         channel);
        uiText.QueueLineEnd(                        channel);

        uiText.QueueText(       " ",                channel);
        uiText.QueueCursor(                         channel);
        uiText.QueueText(       " Sensitivity",     channel);
        uiText.QueueBar(        0.3f, 16,           channel);
    }
    void Open() {
        isOpen = true;
        uiText.OpenChannel(                 channel);
        uiText.SetCursor(1,                 channel);
        uiText.SetStyleFontSize(fontSize,   channel);
        uiText.SetStyleAlignment(alignment, channel);
    }
    void Close() {
        isOpen = false;
        uiText.ClearQueue(  channel);
        uiText.CloseChannel(channel);
    }
}
