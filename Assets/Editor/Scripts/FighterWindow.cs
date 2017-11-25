using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Callbacks;

namespace FightingGame
{
    public class FighterWindow : EditorWindow
    {

        public FighterObject fighter;
        private List<FighterStateEditor> states = new List<FighterStateEditor>();
        Vector2 windowPos = new Vector2();

        private int selectedMove = -1;

        [OnOpenAssetAttribute(1)]
        public static bool step1(int instanceID, int line)
        {
            UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if(obj is FighterObject)
            {
                FighterWindow window = EditorWindow.GetWindow<FighterWindow>();
                window.SetFighter((FighterObject)obj);
                window.states.Clear();
                return true;
            }
            return false; // we did not handle the open
        }

        [MenuItem("Assets/Create/Fighter")]
        public static void CreateMyAsset()
        {
            FighterObject asset = ScriptableObject.CreateInstance<FighterObject>();

           // AssetDatabase.CreateAsset(asset, "Assets/Fighter.asset");
            //AssetDatabase.SaveAssets();
            ProjectWindowUtil.CreateAsset(asset, "Assets/Fighter.asset");

            //EditorUtility.FocusProjectWindow();

            //Selection.activeObject = asset;
        }

        [MenuItem("Window/Fighter Editor %#e")]
        static void Init()
        {
            FighterWindow window = EditorWindow.GetWindow<FighterWindow>();
            window.states.Clear();
        }

        void SetFighter(FighterObject f)
        {
            fighter = f;
        }

        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Fighter Editor", EditorStyles.boldLabel);

            fighter = (FighterObject)EditorGUILayout.ObjectField(fighter, typeof(FighterObject), true);

            GUILayout.EndHorizontal();
            if (!fighter)
            {
                return;
            }

            windowPos = GUILayout.BeginScrollView(windowPos);

            fighter.jumpStrength = EditorGUILayout.FloatField("Jump Strength", fighter.jumpStrength);
            fighter.speed = EditorGUILayout.FloatField("Speed", fighter.speed);

            //Fighter Properties
            GUILayout.BeginHorizontal();
            fighter.Stand = EditorGUILayout.Popup("Stand", fighter.Stand, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Crouch = EditorGUILayout.Popup("Crouch", fighter.Crouch, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Walk = EditorGUILayout.Popup("Walk", fighter.Walk, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            int newMove = EditorGUILayout.Popup(selectedMove, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            if (selectedMove != newMove)
            {
                selectedMove = newMove;
                states.Clear();
                foreach(FighterState f in fighter.moves[selectedMove].frames)
                {
                    states.Add(new FighterStateEditor(f));
                }
            }
            if (selectedMove >= 0)
            {
                fighter.moves[selectedMove].name = GUILayout.TextField(fighter.moves[selectedMove].name);
            }
            if(GUILayout.Button("New Move"))
            {
                fighter.moves.Add(new Move());
                selectedMove = fighter.moves.Count - 1;
            }
            if (GUILayout.Button("Delete"))
            {
                fighter.moves.RemoveAt(selectedMove);
                if (selectedMove >= fighter.moves.Count)
                {
                    selectedMove = fighter.moves.Count - 1;
                }
            }
            GUILayout.EndHorizontal();
            if (selectedMove >= 0)
            {
                //Draw Preview
                FighterStateEditor.DrawState(fighter.moves[selectedMove].GetFrame(Time.time), fighter.moves[selectedMove].GetMaxHeight());

                if(GUILayout.Button("Add State"))
                {
                    FighterState s = new FighterState();
                    List<FighterState> st = fighter.moves[selectedMove].frames;
                    if (st.Count > 0)
                    {
                        s.CopyFrom(st[st.Count-1]);
                    }
                    fighter.moves[selectedMove].frames.Add(s);
                    states.Add(new FighterStateEditor(s));
                }
                GUILayout.BeginHorizontal();
                for(int i = 0; i < states.Count; i++)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        fighter.moves[selectedMove].frames.RemoveAt(i);
                        states.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        states[i].OnGUI();
                    }
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            EditorUtility.SetDirty(fighter);
        }

        private void Update()
        {
            Repaint();
        }

        static string MoveToName(Move m)
        {
            return m.name;
        }

        void OpenItemList()
        {
            
        }
    }
}