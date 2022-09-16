using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteAlways]
public class UiTextScript : MonoBehaviour {
    [Header("Text")]
    public string text       = "";
    public Color textColor   = Color.white;
    public int textSize      = 16;

    Text textBox;
    RawImage container;
    
    string renderedText;
    int cursorPosition;
    float typingTimer;

    string textPrime;

    [HideInInspector] public string textBuffer;

    void Start() {
        textBox = GetComponent<Text>();
    }

    void Update() {
        textBox.text = "";
        //if(typingSpeed == -1) {
            if(text.Contains("\\")) {
                InsertOperatiors();
            } else {
                textBox.text = text + textBuffer;
            }
        //}
        textBox.fontSize = textSize;
        textBox.color = textColor;

        textBuffer = "";
    }
    void InsertOperatiors() {
        string[] lineSeperator = {"\\n"};
        string[] strings = (text+textBuffer).Split(lineSeperator,128,System.StringSplitOptions.RemoveEmptyEntries);
        for(int x = 0; x < strings.Length; x++) {
            textBox.text += strings[x];
            if(x < strings.Length-1) textBox.text += "\n";
        }
    }
}