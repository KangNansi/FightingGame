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
        Vector2 statesScroll = new Vector2();

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
            if (GUILayout.Button("Edit MoveSet"))
            {
                MoveSetEditor.Init(fighter);
            }
            GUILayout.EndHorizontal();
            if (!fighter)
            {
                return;
            }

            windowPos = EditorGUILayout.BeginScrollView(windowPos);
            GUILayout.BeginVertical(GUILayout.MaxWidth(800));
            GUILayout.BeginVertical("Box");

            GUILayout.BeginHorizontal();
            fighter.jumpStrength = EditorGUILayout.FloatField("Jump Strength", fighter.jumpStrength);
            fighter.speed = EditorGUILayout.FloatField("Speed", fighter.speed);
            fighter.fallTime = EditorGUILayout.FloatField("Fall Time", fighter.fallTime);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            fighter.lifeMax = EditorGUILayout.FloatField("Life", fighter.lifeMax);
            fighter.guardMax = EditorGUILayout.FloatField("Guard", fighter.guardMax);
            fighter.stunMax = EditorGUILayout.FloatField("Stun", fighter.stunMax);
            fighter.parryTime = EditorGUILayout.FloatField("Parry Time", fighter.parryTime);
            fighter.parryPerfectTime = EditorGUILayout.FloatField("Perfect Parry Time", fighter.parryPerfectTime);
            GUILayout.EndHorizontal();

            //Fighter Properties
            GUILayout.BeginHorizontal();
            fighter.Stand = EditorGUILayout.Popup("Stand", fighter.Stand, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Crouch = EditorGUILayout.Popup("Crouch", fighter.Crouch, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Walk = EditorGUILayout.Popup("Walk", fighter.Walk, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Hit = EditorGUILayout.Popup("Hit", fighter.Hit, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Block = EditorGUILayout.Popup("Block", fighter.Block, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Taunt = EditorGUILayout.Popup("Taunt", fighter.Taunt, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.Fall = EditorGUILayout.Popup("Fall", fighter.Fall, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.GetUp = EditorGUILayout.Popup("Get Up", fighter.GetUp, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            fighter.JumpStart = EditorGUILayout.Popup("Jump Start", fighter.JumpStart, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.JumpFall = EditorGUILayout.Popup("Jump Fall", fighter.JumpFall, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            fighter.JumpRecovery = EditorGUILayout.Popup("Jump Recovery", fighter.JumpRecovery, fighter.moves.ConvertAll<string>(MoveToName).ToArray());
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

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
                Move selMove = fighter.moves[selectedMove];
                selMove.attack = EditorGUILayout.Toggle("Is Attack?", selMove.attack);
                fighter.moves[selectedMove].velocity = EditorGUILayout.Vector2Field("Move Velocity", fighter.moves[selectedMove].velocity);
                selMove.defaultNext = EditorGUILayout.Popup("Default next:", selMove.defaultNext, fighter.GetMoveList());
                //Draw Preview
                FighterStateEditor.DrawState(fighter.moves[selectedMove].GetFrame((Time.time* fighter.moves[selectedMove].time_modifier)), fighter.moves[selectedMove].GetMaxHeight());
                fighter.moves[selectedMove].time_modifier = EditorGUILayout.Slider(fighter.moves[selectedMove].time_modifier, 0.0f, 5.0f);
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
                
                statesScroll = EditorGUILayout.BeginScrollView(statesScroll);

                GUILayout.BeginHorizontal();
                for(int i = 0; i < states.Count; i++)
                {
                    GUILayout.BeginVertical("Box", GUILayout.MaxWidth(250));
                    GUILayout.BeginHorizontal();
                    if (i > 0 && GUILayout.Button("<"))
                    {
                        states.Insert(i - 1, states[i]);
                        states.RemoveAt(i + 1);
                    }
                    if (GUILayout.Button("Remove"))
                    {
                        fighter.moves[selectedMove].frames.RemoveAt(i);
                        states.RemoveAt(i);
                        i--;
                    }
                    if (i < states.Count-1 && GUILayout.Button(">"))
                    {
                        Debug.Log("insert");
                        states.Insert(i+2, states[i]);
                        states.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                    states[i].OnGUI();
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
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