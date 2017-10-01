using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace FightingGame
{
    public class MoveSetEditor : EditorWindow {
        public Rect windowRect = new Rect(100, 100, 20, 20);
        MoveSet _set;
        Fighter _fighter;
        Action addSet = new Action();
        public static void Init(MoveSet set, Fighter fighter)
        {
            MoveSetEditor window = (MoveSetEditor)EditorWindow.GetWindow(typeof(MoveSetEditor));
            window.Show();
            window._set = set;
            window._fighter = fighter;
        }

        private void OnGUI()
        {
            BeginWindows();
            windowRect = GUILayout.Window(1, windowRect, AddWindow, "Add");
            EndWindows();
        }

        private void AddWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Input:");
            addSet.input = GUILayout.TextField(addSet.input);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("State:");
            addSet.state = EditorGUILayout.Popup(addSet.state, _fighter.GetStateList());
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Add"))
            {
                _set.AddAction(new Action(addSet));
            }
            GUI.DragWindow();
        }
    }
}
