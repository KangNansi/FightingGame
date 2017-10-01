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
        MoveEditor move;
        private void OnEnable()
        {
            f = target as Fighter;
            
            //SceneView.lastActiveSceneView.pivot = f.transform.position + Vector3.back * 4;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            f.opponent = (GameObject)EditorGUILayout.ObjectField(f.opponent, typeof(GameObject), true);
            f.moveSet = (MoveSet)EditorGUILayout.ObjectField(f.moveSet, typeof(MoveSet), false);
            if(GUILayout.Button("Edit MoveSet"))
            {
                MoveSetEditor.Init(f.moveSet, f);
            }

            f.Stand = EditorGUILayout.Popup(f.Stand, f.GetMoveList());

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                Move m = ScriptableObject.CreateInstance<Move>();
                m.name += Random.Range(0, 1000);
                f.moves.Add(m);
            }
            if (GUILayout.Button("Remove"))
            {
                f.moves.RemoveAt(currentMove);
                currentMove = 0;
                f.currentState = 0;
                move = (MoveEditor)Editor.CreateEditor(f.moves[currentMove]);
            }
            int n = EditorGUILayout.Popup(currentMove, f.GetMoveList());
            if (n != currentMove || move == null)
            {
                currentMove = n;
                f.currentState = currentMove;
                move = (MoveEditor)Editor.CreateEditor(f.moves[currentMove]);
            }
            EditorGUILayout.EndHorizontal();
            move.OnInspectorGUI();
            
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            Fighter f = target as Fighter;
            
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
