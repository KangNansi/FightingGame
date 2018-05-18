using UnityEngine;
using UnityEditor;
using LZFight;

[CustomEditor(typeof(LZFighterDebugger))]
public class LZFighterDebuggerEditor : Editor {
    LZFIGHTERINPUTEVENT input;
    bool pauseOnLaunch = true;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        LZFighterDebugger debugger = target as LZFighterDebugger;

        input = (LZFIGHTERINPUTEVENT)EditorGUILayout.EnumPopup("input", input);
        pauseOnLaunch = EditorGUILayout.Toggle("pause", pauseOnLaunch);
        if (GUILayout.Button("Launch")) {
            debugger.GetFighter();
        }

    }
}