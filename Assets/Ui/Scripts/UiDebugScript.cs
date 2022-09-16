using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiDebugScript : MonoBehaviour {
    [Header("Framerate")]
    public bool displayFps;
    public bool displayAverageFps;
    public float averagePeriod;
    [Header("Sync")]
    public bool vSync;
    public KeyCode toggleKey;
    [Header("Container")]
    public Color containerColor;

    Text textBox;
    RawImage container;

    float average;
    float averageTotal;
    float averageTime;
    int averageCount;

    void Start() {
        textBox = transform.GetChild(0).GetComponent<Text>();
        container = GetComponent<RawImage>();
    }

    void Update() {
        textBox.text = "";
        if(displayFps) textBox.text += (1/Time.deltaTime).ToString() + "\n";
        if(displayAverageFps) {
            averageTotal += 1/Time.deltaTime;
            averageTime += Time.deltaTime;
            averageCount++;
            if(averageTime >= averagePeriod) {
                average = averageTotal/averageCount;
                averageTotal = 0;
                averageTime = 0;
                averageCount = 0;
            }
            textBox.text += average.ToString() + "\n";
        }
        if(Input.GetKeyDown(toggleKey)) {
            vSync = !vSync;
        }
        if(vSync) {
            QualitySettings.vSyncCount = 1;
        } else {
            QualitySettings.vSyncCount = 0;
        }
        if(textBox.text == "") {
            container.color = Color.clear;
        } else {
            container.color = containerColor;
        }
    }
}
