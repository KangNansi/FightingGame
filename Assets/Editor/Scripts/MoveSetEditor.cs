using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace FightingGame
{
    public class MoveSetEditor : EditorWindow {
        public Rect windowRect = new Rect(100, 100, 20, 20);
        Fighter _fighter;
        int selectedNode = 0;
        int selectedMove = 0;

        public static void Init(Fighter fighter)
        {
            MoveSetEditor window = (MoveSetEditor)EditorWindow.GetWindow(typeof(MoveSetEditor));
            window.Show();
            window._fighter = fighter;
        }

        private void OnGUI()
        {
            MoveSet set = _fighter.moveSet;
            EditorGUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Set"))
            {
                if(set.nodes.FindAll((Node n) => n.moveId == selectedMove).Count<=0)
                {
                    Node n = new Node();
                    n.moveId = selectedMove;
                    set.nodes.Add(n);
                }
            }
            selectedMove = EditorGUILayout.Popup(selectedMove, _fighter.GetMoveList());
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove"))
            {
                set.nodes.RemoveAt(selectedNode);
                selectedNode = -1;
            }
            selectedNode = EditorGUILayout.Popup(selectedNode, NodeToString());
            EditorGUILayout.EndHorizontal();
            if(selectedNode>=0 && selectedNode < set.nodes.Count)
            {
                DrawNode(set.nodes[selectedNode]);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_fighter);
            }
        }

        void DrawNode(Node n)
        {
            if(GUILayout.Button("Add Move Action"))
            {
                n.actions.Add(new Action());
            }
            foreach(Action a in n.actions)
            {
                EditorGUILayout.BeginHorizontal();
                a.input = (VirtualController.Keys)EditorGUILayout.EnumPopup(a.input);
                a.state = EditorGUILayout.Popup(a.state, _fighter.GetMoveList());
                EditorGUILayout.EndHorizontal();
            }
        }

        string[] NodeToString()
        {
            MoveSet set = _fighter.moveSet;
            List<string> r = new List<string>();
            foreach(Node n in set.nodes)
            {
                r.Add(_fighter.moves[n.moveId].name);
            }
            return r.ToArray();
        }


    }
}
