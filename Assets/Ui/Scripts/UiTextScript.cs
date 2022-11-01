using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------------------------------------------------------------------------------------------------------------------
  UI Text Script
	
	Author -  Cole Barach
	Created - 2022.09.05
	Updated - 2022.09.06
	
	Function
        -Stylistic formatting of strings
        -Prioritizing of multiple channels of input data
        -Insertion of specialized strings
----------------------------------------------------------------------------------------------------------------------------------------------*/

public class UiTextScript : MonoBehaviour {
    [Header("Rendering")]
    public bool[]               queueChannels;
    public string[]             queueText;
    public int                  lineCount;
    [Header("Background")]
    public Color                backgroundActiveColor;
    public Color                backgroundPassiveColor;
    [Header("References")]
    public Text                 textComponent;
    public RawImage             backgroundComponent;
    [Header("Formatting")]
    public string[]             barSymbols = {"","░","▒","▓","█"};
    [Header("Debug")]
    public int                  queueIndex;
    public int                  cursorLine;

    void Start() {
        queueText = new string[queueChannels.Length];
    }
    void Update() {
        Dequeue();
        Render();
        ColorBackground();
    }
    void Dequeue() {
        for(int x = 0; x < queueText.Length; x++) {
            if(queueChannels[x]) {
                queueIndex = x;
                return;
            }
        }
        queueIndex = -1;
        return;
    }
    void Render() {
        textComponent.text = "";
        if(queueIndex == -1) return;
        int lineOffset = cursorLine/lineCount * lineCount;
        for(int lineScreen = 0; lineScreen < lineCount; lineScreen++) {
            int lineNumber = lineScreen + lineOffset;
            string text = queueText[queueIndex];
            string line = GetLine(text,lineNumber);
            textComponent.text += FormatText(line, lineNumber) + "\n";
        }
    }
    void ColorBackground() {
        if(textComponent.text != "") {
            backgroundComponent.color = backgroundActiveColor;
        } else {
            backgroundComponent.color = backgroundPassiveColor;
        }
    }
    string GetLine(string input, int lineNumber) {
        input = input.Replace("\\n","\n");
        string[] lines = input.Split('\n');
        if(lineNumber >= lines.Length) return "";
        return lines[lineNumber];
    }
    int GetLineCount(string input) {
        return CountSubstringFrequency(input, "\\n") + 1;
    }
    // ---------------------------------------------------------------------------------------------- Channels
    public void OpenChannel(int channel) {
        queueChannels[channel] = true;
        Dequeue();
    }
    public void CloseChannel(int channel) {
        queueChannels[channel] = false;
        Dequeue();
    }

    // ---------------------------------------------------------------------------------------------- Cursor
    public void SetCursor(int lineNumber, int channel) {
        if(queueIndex != channel) return;
        cursorLine = lineNumber;
    }
    public void MoveCursor(int step, int channel) {
        if(queueIndex != channel) return;
        int cursorTarget = cursorLine;
        for(int x = 0; x < step; x++) {
            cursorTarget++;
            while(!CheckCursorMove(cursorTarget)) {
               if(!CheckCursorBounds(cursorTarget)) return;
               cursorTarget++;
            }
        }
        for(int x = 0; x > step; x--) {
            cursorTarget--;
            while(!CheckCursorMove(cursorTarget)) {
               if(!CheckCursorBounds(cursorTarget)) return;
               cursorTarget--;
            }
        }
        cursorLine = cursorTarget;
    }
    bool CheckCursorMove(int lineNumber) {
        if(!CheckCursorBounds(lineNumber)) return false;
        string text = queueText[queueIndex];
        string line = GetLine(text, lineNumber);
        return !line.Contains("\\NoCursor\\");
    }
    bool CheckCursorBounds(int lineNumber) {
        string text = queueText[queueIndex];
        int lineCount = GetLineCount(text);
        return lineNumber >= 0 && lineNumber < lineCount;
    }
    // --------------------------------------------------------------------------------------------- Style
    public void SetStyleFontSize(int fontSize, int channel) {
        if(queueIndex != channel) return;
        textComponent.fontSize = fontSize;
    }
    public void SetStyleAlignment(TextAnchor alignment, int channel) {
        if(queueIndex != channel) return;
        textComponent.alignment = alignment;
    }
    // --------------------------------------------------------------------------------------------- Formatting
    string FormatText(string input, int lineNumber) {
        // Escape Characters
        input = input.Replace("\\a", "\a"); // Alarm
        input = input.Replace("\\t", "\t"); // Tab
        //input = input.Replace("\\b", "\b"); // Backspace
        // Custom Encoding
        input = FormatBars(input);
        input = FormatCursors(input, lineNumber);
        input = FormatNoCursors(input);
        return input;
    }
    string FormatBars(string input) {
        // Format: "\\BarStart\\Bar0\\*percent*\\Bar1\\*width*\\BarEnd\\"
        int count = CountSubstringFrequency(input,"\\BarStart");
        for(int x = 0; x < count; x++) {
            int indexStart = input.IndexOf("\\BarStart");
            int indexEnd   = input.IndexOf("\\BarEnd\\");
            string barEncoded = input.Substring(indexStart,indexEnd-indexStart+8);
            input             = input.Remove(   indexStart,indexEnd-indexStart+8);
            input             = input.Insert(   indexStart,DecodeBar(barEncoded));
        }
        return input;
    }
    string FormatCursors(string input, int lineNumber) {
        // Format: "\\Cursor\\"
        int count = CountSubstringFrequency(input,"\\Cursor\\");
        for(int x = 0; x < count; x++) {
            int index = input.IndexOf("\\Cursor\\");
            input = input.Remove(index,8);
            if(cursorLine == lineNumber) {
                input = input.Insert(index, "*");
            } else {
                input = input.Insert(index, " ");
            }
        }
        return input;
    }
    string FormatNoCursors(string input) {
        return input.Replace("\\NoCursor\\", "");
    }

    int CountSubstringFrequency(string input, string substring) {
        return (input.Length - input.Replace(substring,"").Length) / substring.Length;
    }

    // --------------------------------------------------------------------------------------------- Queue
    public void QueueText(string text, int channel) {
        queueText[channel] += text;
    }
    public void QueueFloat(float value, int channel) {
        QueueText(value.ToString(), channel);
    }
    public void QueueInt(int value, int channel) {
        QueueText(value.ToString(), channel);
    }
    public void QueueLineEnd(int channel) {
        QueueText("\\n", channel);
    }
    public void QueueBar(float percent, int width, int channel) {
        QueueText(EncodeBar(percent,width), channel);
    }
    public void QueueCursor(int channel) {
        QueueText(EncodeCursor(), channel);
    }
    public void QueueNoCursor(int channel) {
        QueueText("\\NoCursor\\", channel);
    }
    public void StartQueue(int channel) {
        ClearQueue(channel);
    }
    public void ClearQueue(int channel) {
        queueText[channel] = "";
    }

    // --------------------------------------------------------------------------------------------- Encoding/Decoding
    public string EncodeBar(float percent, int width) {
        string bar = "";
        bar += "\\BarStart\\Bar0\\";
        bar += percent.ToString();
        bar += "\\Bar1\\";
        bar += width.ToString();
        bar += "\\BarEnd\\";
        return bar;
    }
    public string EncodeCursor() {
        return "\\Cursor\\";
    }
    public string DecodeBar(string input) {
        int indexPercent = input.IndexOf("\\Bar0\\");
        int indexWidth   = input.IndexOf("\\Bar1\\");
        int indexEnd     = input.IndexOf("\\BarEnd\\");
        float percent = float.Parse(input.Substring(indexPercent+6,indexWidth-indexPercent-6));
        int   width   = int.Parse(  input.Substring(indexWidth  +6,indexEnd  -indexWidth  -6));
        return RenderBar(percent,width);
    }
    public string RenderBar(float percent, int barWidth) {
        string bar = "";
        for(int x = 0; x < barWidth; x++) {
            int symbolIndex = Mathf.RoundToInt(Mathf.Clamp(((percent*barWidth-x)),0,1)*(barSymbols.Length-1));
            bar += barSymbols[symbolIndex];
        }
        return bar;
    }
}