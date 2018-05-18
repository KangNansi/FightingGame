using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LZFighterAnimator))]
public class LZFighterAnimatorEditor : Editor {
    private Editor playerEditor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        LZFighterAnimator animator = target as LZFighterAnimator;

        animator.fighterObject.invertHorizontal = EditorGUILayout.Toggle("Invert Horizontal: ", animator.fighterObject.invertHorizontal);

        SerializedObject serialized = new SerializedObject(animator.controller);
        if(animator.controller != null) {
            if(playerEditor == null) {
                playerEditor = CreateEditor(animator.controller, typeof(LZFightPlayerEditor));
            }
            EditorGUILayout.BeginVertical(GUITools.PaddedNodeStyle);
            playerEditor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }

    }
}