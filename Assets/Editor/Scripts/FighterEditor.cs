using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FightingGame
{
    [CustomEditor(typeof(Fighter))]
    public class FighterEditor : Editor {
        Fighter f;
        private void OnEnable()
        {
            f = target as Fighter;
            //SceneView.lastActiveSceneView.pivot = f.transform.position + Vector3.back * 4;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            if(GUILayout.Button("Edit MoveSet"))
            {
                MoveSetEditor.Init(f.moveSet, f);
            }
            
            if(GUILayout.Button("Add State"))
            {
                f.states.Add(new FighterState());
            }
            f.currentState = EditorGUILayout.Popup(f.currentState, f.GetStateList());
            if(GUILayout.Button("Add Hitbox"))
            {
                f.states[f.currentState].hitboxes.Add(new HitBox(HitBox.Type.Attack, Vector2.zero, Vector2.one));
            }
            DrawState();
            if (GUI.changed)
                SceneView.RepaintAll();
        }

        private void OnSceneGUI()
        {
            Fighter f = target as Fighter;
            
        }

        private void DrawState()
        {
            FighterState state = f.states[f.currentState];
            for(int i=state.hitboxes.Count-1; i>=0; i--)
            {
                HitBox h = state.hitboxes[i];
                EditorGUILayout.Separator();
                h._Type = (HitBox.Type)EditorGUILayout.Popup((int)h._type, System.Enum.GetNames(typeof(HitBox.Type)));
                h._position = EditorGUILayout.Vector2Field("Pos:", h._position);
                h._size = EditorGUILayout.Vector2Field("Size:", h._size);
                if (GUILayout.Button("Remove"))
                {
                    state.hitboxes.Remove(h);
                }
            }
        }
    }
}
