using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using LZFight;
using InputManager;

public class LZFighterEditorInspector {
    public LZFighter fighter;
    private GUIStyle titleStyle = null;
    private GUIStyle TitleStyle {
        get {
            if(titleStyle == null) {
                titleStyle = new GUIStyle();
                titleStyle.alignment = TextAnchor.MiddleCenter;
                titleStyle.fontSize = 32;
                titleStyle.fixedHeight = 44;
                titleStyle.normal.textColor = Color.white;
                titleStyle.fontStyle = FontStyle.Bold;
            }
            return titleStyle;
        }
    }

    public void OnInspectorGUI(Rect rect, StateMachineEditor graph) {
        GUILayout.BeginArea(rect, GUI.skin.box);
        EditorGUILayout.BeginVertical();
        object selected = graph.GetSelected();
        if (selected is StateMachineNode) {
            DrawState((StateMachineNode)selected);
        }
        if (selected is LZFighterStateTransition) {
            DrawTransition((LZFighterStateTransition)selected);
        }
        EditorGUILayout.EndVertical();

        ProcessEvents(Event.current);
        GUILayout.EndArea();
    }

    public void ProcessEvents(Event ev) {
        int controlID = EditorGUIUtility.GetControlID(FocusType.Keyboard);
        switch (ev.type) {
            case EventType.MouseDown:
                if(ev.button == 0) {
                    ev.Use();
                }
                break;
            case EventType.mouseUp:
                    ev.Use();
                break;
        }

    }

    public void DrawState(StateMachineNode state) {
        if (state.IsShortcut) {
            DrawShortcut(state);
            return;
        }
        LabelTitle(state.name);
        state.data = (LZFighterStateData)EditorGUILayout.ObjectField(state.data, typeof(LZFighterStateData), false);
        if(state.data != null && state.name == "") {
            state.name = state.data.name;
        }
        // Edit instance
        if (GUILayout.Button("Edit")) {
            if(state.data == null) {
                LZFighter fighter = LZFighterEditor.Instance.fighter;
                string path = AssetDatabase.GetAssetPath(fighter);
                string filename = Path.GetFileNameWithoutExtension(path);
                string directory = Path.GetDirectoryName(path);
                if (!AssetDatabase.IsValidFolder(directory + "/" + filename)) {
                    AssetDatabase.CreateFolder(directory, filename);
                }
                LZFighterStateData newData = ScriptableObject.CreateInstance<LZFighterStateData>();
                AssetDatabase.CreateAsset(newData, directory + "/" + filename + "/" + state.name + ".asset");
                Debug.Log(Path.GetDirectoryName(path));
                state.data = newData;
            }
            LZFighterStateEditor.Open(state.data);
        }
        string currentName = state.name;
        string newName = EditorGUILayout.TextField("Name:", currentName);
        /*if (currentName != newName && state.data != null) {
            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(state.data), newName);
        }*/
        state.name = newName;
        state.velocity = EditorGUILayout.Vector2Field("StateVelocity: ", state.velocity);
        if(state.data != null) {
            state.data.velocity = EditorGUILayout.Vector2Field("DataVelocity:", state.data.velocity);
        }
        state.invert = EditorGUILayout.Toggle("Invert", state.invert);
        state.loop = EditorGUILayout.Toggle("Loop", state.loop);
    }

    private void DrawShortcut(StateMachineNode node) {
        StateMachineNode target = fighter.stateMachine.states[node.Target];
        node.name = target.name;
        DrawState(target);
    }

    private void DrawTransition(LZFighterStateTransition transition) {
        //EditorGUILayout.LabelField("Target: " + fighter.states[transition.target].stateName);
        EditorGUILayout.LabelField("Event", EditorStyles.centeredGreyMiniLabel);
        for(int i = transition.events.Count-1; i >= 0; i--) {
            EditorGUILayout.BeginHorizontal();
            transition.events[i] = (LZFIGHTEREVENT)EditorGUILayout.EnumPopup(transition.events[i]);
            if (GUILayout.Button("delete")) {
                transition.events.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+")) {
            transition.events.Add(LZFIGHTEREVENT.NONE);
        }

        ///  INPUTS
        EditorGUILayout.LabelField("Inputs", EditorStyles.centeredGreyMiniLabel);
        if (transition.inputs == null) {
            transition.inputs = new List<LZFighterStateTransition.Input>();
        }
        for (int i = transition.inputs.Count - 1; i >= 0; i--) {
            EditorGUILayout.BeginHorizontal();
            transition.inputs[i].no = GUITools.NoField("!", transition.inputs[i].no);
            transition.inputs[i].type = (LZFighterStateTransition.INPUT_TYPE)EditorGUILayout.EnumPopup(transition.inputs[i].type);
            transition.inputs[i].ev = (LZFIGHTERINPUTEVENT)EditorGUILayout.EnumPopup(transition.inputs[i].ev);
            if (GUILayout.Button("delete")) {
                transition.inputs.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+")) {
            transition.inputs.Add(new LZFighterStateTransition.Input());
        }

        /// Conditions
        EditorGUILayout.LabelField("Scripts", EditorStyles.centeredGreyMiniLabel);
        GUITools.ListField<Condition>(transition.scriptedConditions, (i) => {
            transition.scriptedConditions[i] = (Condition)EditorGUILayout.ObjectField(transition.scriptedConditions[i], typeof(Condition), false);
        });

        /// SCRIPTS
        EditorGUILayout.LabelField("Scripts", EditorStyles.centeredGreyMiniLabel);
        GUITools.ScriptListField(transition.scripts, fighter);
        /*GUITools.ListField<MiniScript>(transition.scripts, (i) => {
            GUITools.ScriptField(transition.scripts[i], null);
            transition.scripts[i] = (MiniScript)EditorGUILayout.ObjectField(transition.scripts[i], typeof(MiniScript), false);
        });*/

        
    }

    public void LabelTitle(string name) {
        EditorGUILayout.BeginVertical(GUITools.NodeStyle, GUILayout.Height(TitleStyle.fixedHeight));
        EditorGUILayout.LabelField(name, TitleStyle);
        EditorGUILayout.EndVertical();
    }

}
