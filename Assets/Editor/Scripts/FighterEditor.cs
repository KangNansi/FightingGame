using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FightingGame
{
    [CustomEditor(typeof(Fighter))]
    public class FighterEditor : Editor {
        Fighter f;
        int currentMove = 0;
        int currentFrame = 0;
        
        private void OnEnable()
        {
            f = target as Fighter;
            
            //SceneView.lastActiveSceneView.pivot = f.transform.position + Vector3.back * 4;
        }

        public override void OnInspectorGUI()
        {
            Undo.RecordObject(target, "obj");
            f.drawHitbox = EditorGUILayout.Toggle(f.drawHitbox);
            //base.OnInspectorGUI();
            f.controllerNumber = EditorGUILayout.IntField(f.controllerNumber);
            f.opponent = (GameObject)EditorGUILayout.ObjectField(f.opponent, typeof(GameObject), true);
            //f.moveSet = (MoveSet)EditorGUILayout.ObjectField(f.moveSet, typeof(MoveSet), false);
            if(GUILayout.Button("Edit MoveSet"))
            {
                MoveSetEditor.Init(f);
            }

            f.jumpStrength = EditorGUILayout.FloatField("Jump Strength", f.jumpStrength);
            f.speed = EditorGUILayout.FloatField("Speed", f.speed);

            f.Stand = EditorGUILayout.Popup("Stand:", f.Stand, f.GetMoveList());
            f.Crouch = EditorGUILayout.Popup("Crouch:", f.Crouch, f.GetMoveList());

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                Move m = new Move();
                m.name += Random.Range(0, 1000);
                f.moves.Add(m);
            }
            if (GUILayout.Button("Remove"))
            {
                f.moves.RemoveAt(currentMove);
                currentMove = 0;
                f.currentState = 0;
            }
            int n = EditorGUILayout.Popup(currentMove, f.GetMoveList());
            if (n != currentMove)
            {
                Debug.Log("lol");
                currentMove = n;
                f.currentState = currentMove;

            }
            Debug.Log(f.currentState);
            EditorGUILayout.EndHorizontal();
            DrawMove(f.moves[currentMove]);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                SceneView.RepaintAll();
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {

            
        }

        private void DrawMove(Move t)
        {
            t.name = EditorGUILayout.TextField("Name:", t.name);
            string[] frames = new string[t.frames.Count];
            for (int i = 0; i < t.frames.Count; i++)
            {
                frames[i] = "Frame " + i;
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                t.frames.Add(new FighterState());
            }
            if (GUILayout.Button("Remove"))
            {
                t.frames.RemoveAt(currentFrame);
                currentFrame = 0;
            }
            currentFrame = EditorGUILayout.Popup(currentFrame, frames);
            t.currentFrame = currentFrame;
            EditorGUILayout.EndHorizontal();
            if (t.frames.Count > 0 && currentFrame < t.frames.Count)
                DrawFrame(t.frames[currentFrame]);
        }

        void DrawFrame(FighterState state)
        {
            state.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite:", state.sprite, typeof(Sprite), false);
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
        /*
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
        }*/
    }
}
