using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[UnityEditor.CustomPropertyDrawer(typeof(UUIDProperty))]
public class UUIDDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        UUIDProperty prop = (UUIDProperty)attribute;

        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use UUIDProperty with string.");
            return;
        }

        GUI.enabled = false;

        if (property.stringValue.Equals(""))
        {
            Debug.Log("UUID empty");
            var uuid = System.Guid.NewGuid().ToString();
            Debug.Log("new:" + uuid);
            property.stringValue = uuid;
        }

        EditorGUI.PropertyField(new Rect(position.x+80, position.y, 630, 15), property, label, false);
        GUI.enabled = true;
        if (GUI.Button(new Rect(position.x,position.y,70,15),"Generate"))
        {
            Debug.Log("Serialized:"+property.stringValue);
            var uuid = System.Guid.NewGuid().ToString();
            Debug.Log("new:" + uuid);
            property.stringValue = uuid;
        }
        
    }
}
