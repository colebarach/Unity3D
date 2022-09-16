using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

[ExecuteInEditMode]
public class RenderOverrideResolution : MonoBehaviour {
    public enum Orientation {
        vertical = 0,
        horizontal = 1
    };
    
    [ReadOnly]
    public Vector2Int dimensions;
    [Header("")]
    public Orientation ScalarAxis;
    public int scale;
    RenderOverrideMaster master;

    void Start() {
        master = GetComponent<RenderOverrideMaster>();
    }

    void Update() {
        if(ScalarAxis == 0) {
            dimensions.y = scale;
            dimensions.x = (int)((Screen.width*1.0f/Screen.height)*scale);
        } else {
            dimensions.x = scale;
            dimensions.y = (int)((Screen.height*1.0f/Screen.width)*scale);
        }
        master.dimensionOverride = dimensions;
    }
}

[AttributeUsage (AttributeTargets.Field,Inherited = true)]
public class ReadOnlyAttribute : PropertyAttribute {}

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer (typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
	public override void OnGUI(Rect rect, UnityEditor.SerializedProperty prop, GUIContent label)
	{
		bool wasEnabled = GUI.enabled;
		GUI.enabled = false;
		UnityEditor.EditorGUI.PropertyField(rect,prop);
		GUI.enabled = wasEnabled;
	}
}
#endif
