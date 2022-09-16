using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTextProgressBarOverride : MonoBehaviour {
    [Header("ProgressBar")]
    public float percent     = 0;
    public int barWidth      = 16;
    public string[] progressSymbols = {"","░","▒","▓","█"};

    UiTextScript textBox;

    void Start() {
        textBox = GetComponent<UiTextScript>();
    }

    void Update() {
        if(percent != 0) {
            if(textBox.text != "" && textBox.textBuffer != "") textBox.textBuffer += "\n";
            for(int x = 0; x < barWidth; x++) {
                int index = Mathf.RoundToInt(Mathf.Clamp(((percent*barWidth-x)),0,1)*(progressSymbols.Length-1));;
                textBox.textBuffer += progressSymbols[index];
            }
        }
    }
}