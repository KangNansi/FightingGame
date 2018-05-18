using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using LZFight;
using LZFight.Scripting;
/*
[AttributeUsage(AttributeTargets.Field)]
public class ListAttribute : PropertyAttribute {

}

[CustomPropertyDrawer(typeof(int))]
public class FighterStateDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        //base.OnGUI(position, property, label);
        int size = property.arraySize;
        for(int i = 0; i < size; i++) {
            EditorGUI.PropertyField(position, property.GetArrayElementAtIndex(i));
        }
    }
}*/