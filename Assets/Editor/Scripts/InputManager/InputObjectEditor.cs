using UnityEngine;
using UnityEditor;
/*
[CustomEditor(typeof(InputObject))]
public class InputObjectEditor : Editor {
    public override void OnInspectorGUI() {
        InputObject obj = target as InputObject;

        if(obj == null) {
            if (GUILayout.Button("Add")) {
                obj = new KeyInput();
            }
        }
        else {
            KeyInput input = obj as KeyInput;
            input.code = (KeyCode)EditorGUILayout.EnumPopup(input.code);
        }
    }

    public static InputObject InputObjectField(InputObject obj) {
        if (obj == null) {
            if (GUILayout.Button("Add")) {
                //obj = Object.Instantiate();
            }
        }
        else {
            KeyInput input = obj as KeyInput;
            input.code = (KeyCode)EditorGUILayout.EnumPopup(input.code);
        }
        return obj;
    }
}*/