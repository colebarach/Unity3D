using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiInventoryScript : MonoBehaviour {
    [Header("Input")]
    public KeyCode                  openKey;
    public KeyCode                  cursorUpKey;
    public KeyCode                  cursorDownKey;
    public KeyCode[]                closeOverrides;
    [Header("Style")]
    public int                      fontSize;
    public TextAnchor               alignment;
    [Header("References")]
    public PlayerInventory          inventory;
    public UiTextScript             uiText;
    public int                      channel;
    
    public bool                     isOpen;

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
        uiText.StartQueue(                                      channel);
        uiText.QueueNoCursor(                                   channel);
        uiText.QueueText(       "Inventory:",                   channel);

        for(int x = 0; x < inventory.itemNames.Length; x++) {
            if(inventory.itemQuantities[x] != 0) {
                uiText.QueueLineEnd(                            channel);
                uiText.QueueText(" ",                           channel);
                uiText.QueueCursor(                             channel);
                uiText.QueueText(" ",                           channel);
                uiText.QueueText(inventory.itemNames[x],        channel);
                uiText.QueueText(" (",                          channel);
                uiText.QueueInt(inventory.itemQuantities[x],    channel);
                uiText.QueueText(")",                           channel);
            }
        }
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
