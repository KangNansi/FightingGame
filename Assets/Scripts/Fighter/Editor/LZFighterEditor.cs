using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using LZFight;
using System.Collections.Generic;

public class LZFighterEditor : EditorWindow {
    public LZFighter fighter;

    public SerializedProperty initScriptsProperty;
     
    private Vector2 scrollPosition;

    private List<int> statePath = new List<int>();

    GUIStyle leftStyle;
    private static GUIStyle nodeStyle;
    public static GUIStyle NodeStyle {
        get {
            if(nodeStyle == null) {
                nodeStyle = GUI.skin.GetStyle("flow node 1");
                nodeStyle.padding = new RectOffset(10, 10, 10, 10);
                nodeStyle.border = new RectOffset(10, 10, 10, 10);
            }
            return nodeStyle;
        }
    }
    public static LZFighterEditor instance;
    public static LZFighterEditor Instance {
        get {
            if(instance == null) {
                instance = EditorWindow.GetWindow<LZFighterEditor>();
            }
            return instance;
        }
    }
    

    private StateMachineEditor graphEditor;


    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line) {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is LZFighter) {
            LZFighterEditor window = EditorWindow.GetWindow<LZFighterEditor>();
            window.fighter = (LZFighter)obj;
            window.Init();
            return true;
        }
        return false; // we did not handle the open
    }

    private void Init() {
        leftStyle = GUI.skin.box;
        
        instance = this;
    }

    public static void RepaintWindow() {
        LZFighterEditor window = EditorWindow.GetWindow<LZFighterEditor>();
        if(window != null) {
            window.Repaint();
        }
    }

    private void SetStateEditor() {
        graphEditor = new StateMachineEditor(fighter.stateMachine, fighter);
    }

    private void OnGUI() {

        Event ev = Event.current;
        if(graphEditor == null) {
            SetStateEditor();
        }

        Draw();
    }

    

    void Draw() {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginArea(new Rect(0, 0, 300, position.height), leftStyle);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(300));
        fighter.fighterName = EditorGUILayout.TextField("Name", fighter.fighterName);
        fighter.internalVelocityStrength = EditorGUILayout.FloatField("VelocityMultiplier", fighter.internalVelocityStrength);

        if(GUILayout.Button("Clean Transitions", EditorStyles.toolbarButton)) {
            fighter.stateMachine.CleanTransitions();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUITools.ListField(fighter.initScripts, (i) => {
            EditorGUILayout.BeginVertical(GUITools.NodeStyle);
            fighter.initScripts[i] = (MiniScript)EditorGUILayout.ObjectField(fighter.initScripts[i], typeof(MiniScript), false);
            if (fighter.initScripts[i] != null) {
                fighter.initScripts[i].Initialize(fighter);
                Editor scriptEditor = Editor.CreateEditor(fighter.initScripts[i]);
                scriptEditor.OnInspectorGUI();
            }
            EditorGUILayout.EndVertical();
        });
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();

        Rect graphRect = new Rect(300, 30, position.width - 300, position.height-30);

        
        graphEditor.Draw(graphRect);
        

        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(fighter);
        }
    }

    

}