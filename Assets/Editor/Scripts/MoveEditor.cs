using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace FightingGame
{
[CustomEditor(typeof(Move))]
public class MoveEditor : Editor {
        int currentFrame = 0;
        Move t;

        private void OnEnable()
        {
            t = target as Move;
        }
        public override void OnInspectorGUI()
        {
            t.name = EditorGUILayout.TextField("Name:",t.name);
            string[] frames = new string[t.frames.Count];
            for(int i = 0; i < t.frames.Count; i++)
            {
                frames[i] = "Frame " + i;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                t.frames.Add(ScriptableObject.CreateInstance<FighterState>());
            }
            if (GUILayout.Button("Remove"))
            {
                t.frames.RemoveAt(currentFrame);
                currentFrame = 0;
            }
            currentFrame = EditorGUILayout.Popup(currentFrame, frames);
            t.currentFrame = currentFrame;
            EditorGUILayout.EndHorizontal();
            if (t.frames.Count>0)
                DrawFrame();
        }

        void DrawFrame()
        {
            FighterState state = t.frames[currentFrame];
            state.time = EditorGUILayout.Slider(state.time, 0, 10);
            if (GUILayout.Button("Add Hitbox"))
            {
                state.hitboxes.Add(new HitBox(HitBox.Type.Attack, Vector2.zero, Vector2.one));
            }
            for (int i = state.hitboxes.Count - 1; i >= 0; i--)
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
