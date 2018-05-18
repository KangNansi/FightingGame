using UnityEngine;
using UnityEditor;
using LZFight;
using System.Collections.Generic;

public class StateMachineEditor {
    public StateMachine machine;
    public LZFighter fighter;

    public Vector2 graphOffset;
    private bool dragging = false;

    private enum STATE {
        IDLE, MAKING_TRANSITION
    }
    private STATE state = STATE.IDLE;

    private enum SELECTED_TYPE {
        STATE, TRANSITION
    }
    private SELECTED_TYPE selectedType = SELECTED_TYPE.STATE;
    private bool hasSelected = false;

    public System.Action<int> OnNodeSelected;

    public List<int> treePath = new List<int>();
    public List<StateMachineNode> selectedStates = new List<StateMachineNode>();
    public List<LZFighterStateTransition> selectedTransitions = new List<LZFighterStateTransition>();
    private StateMachineNode transitionStart = null;

    private LZFighterEditorInspector inspector;

    public StateMachineEditor(StateMachine machine, LZFighter fighter) {
        this.machine = machine;
        this.fighter = fighter;
    }

    public object GetSelected() {
        if (hasSelected) {
            if(selectedType == SELECTED_TYPE.STATE) {
                return selectedStates[0];
            }
            else {
                return selectedTransitions[0];
            }
        }
        return null;
    }

    public void Draw(Rect rect) {

        GUILayout.BeginArea(rect, GUI.skin.box);
        EditorGUI.DrawRect(new Rect(Vector2.zero, rect.size), new Color(0.3f, 0.3f, 0.3f));
        GUITools.DrawGrid(rect, graphOffset, 5f, 0.2f, Color.gray * 0.5f);
        GUITools.DrawGrid(rect, graphOffset, 20f, 0.5f, Color.gray);


        DrawTreePath(new Rect(0, 0, 1000, 25));
        DrawTransitions();
        DrawNodes();
        

        if (hasSelected) {
            if (inspector == null) {
                inspector = new LZFighterEditorInspector();
            }
            inspector.fighter = fighter;
            inspector.OnInspectorGUI(new Rect(rect.size.x - 300, 0, 300, rect.size.y), this);
        }

        ProcessEvents();



        GUILayout.EndArea();
    }

    private void DrawTreePath(Rect rect) {
        GUILayout.BeginArea(rect);
        EditorGUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("Master", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false), GUILayout.MinWidth(100))) {
            treePath.Clear();
        }
        for (int i = 0; i < treePath.Count; i++) {
            if (GUILayout.Button("" + fighter.stateMachine[treePath[i]].name, EditorStyles.toolbarButton, GUILayout.ExpandWidth(false), GUILayout.MinWidth(100))) {
                treePath.RemoveRange(i + 1, treePath.Count - i - 1);
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawNodes() {
        //Draw nodes
        foreach (StateMachineNode state in GetCurrentStates()) {
            GUILayout.BeginArea(state.nodeRect);
            int index = machine.states.FindIndex((s) => s == state);
            bool start = false;
            if (treePath.Count > 0) {
                start = index == machine.states[treePath[treePath.Count - 1]].startState;
            }
            else {
                start = index == machine.startState;
            }
            
            GUIStyle style;
            int id = 1;
            if (state.IsShortcut) {
                id = 0;
            }
            if (start) {
                id = 2;
            }
            if (selectedStates.Contains(state) && selectedType == SELECTED_TYPE.STATE) {
                style = GUITools.GetNodeStyle(id, true);
            }
            else {
                style = GUITools.GetNodeStyle(id, false);
            }
            GUI.SetNextControlName("Node");
            using (var h = new EditorGUILayout.HorizontalScope(GUITools.PaddedContainer)) {
                if (state.IsShortcut) {
                    GUI.color = Color.magenta;
                }
                GUILayout.BeginVertical(style);
                GUI.color = Color.white;

                GUILayout.Label(state.name, GUITools.MiddleText);
                Vector2 size = GUITools.MiddleText.CalcSize(new GUIContent(state.name));
                GUILayout.EndVertical();
                Rect vRect = GUILayoutUtility.GetLastRect();

                if (Event.current.type == EventType.Repaint) {
                    state.nodeRect.height = h.rect.height;
                    state.nodeRect.width = Mathf.Max(150, size.x + 25);
                }
            }


            ProcessEvents(state);
            GUILayout.EndArea();
        }
    }

    private void DrawTransitions() {
        Handles.BeginGUI();
        foreach (LZFighterStateTransition t in GetCurrentTransitions()) {
            StateMachineNode source = machine[t.source];
            StateMachineNode target = machine[t.target];
            Vector2 startP = source.nodeRect.center;
            Vector2 endP = target.nodeRect.center;
            Vector2 dir = endP - startP;
            Vector2 rotatedDir = dir.normalized.Rotated(90);
            startP += rotatedDir * 7;
            endP += rotatedDir * 7;
            if (selectedTransitions.Contains(t) && selectedType == SELECTED_TYPE.TRANSITION) {
                Handles.color = Color.cyan;
            }
            else {
                Handles.color = Color.white;
            }
            Handles.DrawLine(startP, endP);
            GUITools.DrawArrow(startP + dir / 2f + dir.normalized * 5f, dir.normalized);
            Handles.color = Color.white;
        }
        if (state == STATE.MAKING_TRANSITION) {
            Handles.DrawLine(transitionStart.nodeRect.position + transitionStart.nodeRect.size / 2f, Event.current.mousePosition);
        }
        Handles.EndGUI();
    }

    public void ProcessEvents() {
        Event ev = Event.current;
        switch (ev.type) {
            case EventType.MouseDown:
                if (ev.button == 0) {
                    //Select Transition
                    foreach(LZFighterStateTransition t in GetCurrentTransitions()) {
                        Vector2 A = machine[t.source].nodeRect.center;
                        Vector2 B = machine[t.target].nodeRect.center;
                        Vector2 dirMod = (B - A).normalized.Rotated(90);
                        A += dirMod * 7;
                        B += dirMod * 7;
                        if (GUITools.SegmentPointDistance(A, B, ev.mousePosition) < 7) {
                            selectedType = SELECTED_TYPE.TRANSITION;
                            if (!ev.shift) {
                                selectedTransitions.Clear();
                            }
                            selectedTransitions.Add(t);
                            selectedStates.Clear();
                            hasSelected = true;
                            ev.Use();
                            return;
                        }
                    }

                    foreach (StateMachineNode state in GetCurrentStates()) {
                        state.isDragged = true;
                    }
                    selectedTransitions.Clear();
                    selectedStates.Clear();
                    hasSelected = false;
                    dragging = true;
                    LZFighterEditor.RepaintWindow();
                }
                if (ev.button == 1) {
                    ProcessContextMenu(ev.mousePosition, fighter);
                }
                break;

            case EventType.MouseUp:
                foreach (StateMachineNode state in GetCurrentStates()) {
                    state.isDragged = false;
                }
                EndTransition();
                dragging = false;
                break;

            case EventType.MouseDrag:
                foreach (StateMachineNode state in GetCurrentStates()) {
                    if (state.isDragged) {
                        state.nodeRect.position += ev.delta;
                    }
                }
                if (dragging) {
                    graphOffset += ev.delta;
                }
                ev.Use();
                break;

            case EventType.KeyDown:
                switch (ev.keyCode) {
                    case KeyCode.Delete:
                        if(selectedType == SELECTED_TYPE.TRANSITION) {
                            foreach(var t in selectedTransitions) {
                                machine.transitions.Remove(t);
                            }
                        }
                        else {
                            foreach(var s in selectedStates) {
                                machine.RemoveState(s);
                            }
                        }
                        ev.Use();
                        break;

                    case KeyCode.S:
                        if (selectedType == SELECTED_TYPE.TRANSITION) {
                            
                        }
                        else {
                            if(treePath.Count > 0) {
                                machine.states[treePath[treePath.Count - 1]].startState = machine.states.FindIndex((s) => s == selectedStates[0]);
                            }
                            else {
                                machine.startState = machine.states.FindIndex((s) => s == selectedStates[0]);
                            }
                        }
                        ev.Use();
                        break;
                }
                break;
        }
    }

    public void ProcessEvents(StateMachineNode fighterState) {
        Event ev = Event.current;
        Rect rect = new Rect(Vector2.zero, fighterState.nodeRect.size);
        switch (ev.type) {
            case EventType.MouseDown:
                if (ev.button == 0 && rect.Contains(ev.mousePosition)) {
                    if (ev.clickCount > 1) {
                        
                        int index = machine.states.FindIndex((s) => s == fighterState);
                        treePath.Add(index);
                        if (OnNodeSelected != null) {
                            OnNodeSelected.Invoke(index);
                        }
                        ev.Use();
                        return;
                        
                    }
                    fighterState.isDragged = true;
                    selectedType = SELECTED_TYPE.STATE;
                    if (!ev.shift) {
                        selectedStates.Clear();
                    }
                    selectedTransitions.Clear();

                    selectedStates.Add(fighterState);
                    hasSelected = true;
                    ev.Use();
                    LZFighterEditor.RepaintWindow();
                    GUI.FocusControl("Node");
                }
                if (ev.button == 1) {
                    BeginTransition(fighterState);
                    ev.Use();
                }
                break;
            case EventType.MouseUp:
                if (state == STATE.MAKING_TRANSITION) {
                    Debug.Log("Making transition between " + transitionStart.name + " and " + fighterState.name);
                    EndTransition(fighterState);
                    ev.Use();
                }
                break;
        }
    }


    private void ProcessContextMenu(Vector2 mousePosition, LZFighter fighter) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition, fighter));
        foreach(var state in machine.states.GetIDs()) {
            string path = "Shortcut/";
            path += machine.GetStatePath(state);
            genericMenu.AddItem(new GUIContent(path), false, () => OnClickAddShortcut(mousePosition, state));
        }
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition, LZFighter fighter) {
        /*string path = AssetDatabase.GetAssetPath(fighter);
        string filename = Path.GetFileNameWithoutExtension(path);
        string directory = Path.GetDirectoryName(path);
        if (!AssetDatabase.IsValidFolder(directory + "/" + filename)) {
            AssetDatabase.CreateFolder(directory, filename);
        }*/
        StateMachineNode newState = new StateMachineNode();
        
        newState.nodeRect.position = mousePosition;
        int id = machine.states.Add(newState);
        if (treePath.Count > 0) {
            StateMachineNode parent = machine.states[treePath[treePath.Count - 1]];
            newState.parent = treePath[treePath.Count - 1];
            parent.containedNodes.Add(id);
            if (parent.containedNodes.Count == 1) {
                parent.startState = id;
            }
        }
        /*AssetDatabase.CreateAsset(newState, directory + "/" + filename + "/" + fighter.states.Count + ".asset");
        Debug.Log(Path.GetDirectoryName(path));*/
    }

    private void OnClickAddShortcut(Vector2 mousePosition, int state) {
        StateMachineNode newState = new StateMachineNode(true, state);
        newState.name = machine.states[state].name;
        newState.nodeRect.position = mousePosition;
        int id = machine.states.Add(newState);
        if (treePath.Count > 0) {
            newState.parent = treePath[treePath.Count - 1];
            machine.states[treePath.Count - 1].containedNodes.Add(id);
        }
    }

    private void BeginTransition(StateMachineNode start) {
        transitionStart = start;
        state = STATE.MAKING_TRANSITION;
    }

    private void EndTransition(StateMachineNode end) {
        state = STATE.IDLE;
        LZFighterStateTransition newTransition = new LZFighterStateTransition();
        newTransition.source = machine.states.FindIndex((s) => s == transitionStart);
        newTransition.target = machine.states.FindIndex((s) => s == end);
        if(newTransition.source != newTransition.target && machine.transitions.Find((t) => t.source == newTransition.source && t.target == newTransition.target) == null) {
            machine.transitions.Add(newTransition);
        }
        
        LZFighterEditor.RepaintWindow();
    }

    private void EndTransition() {
        state = STATE.IDLE;
        LZFighterEditor.RepaintWindow();
    }

    private List<StateMachineNode> GetCurrentStates() {
        return machine.GetChildren(treePath.Count <= 0 ? -1 : treePath[treePath.Count - 1]);
    }

    private List<LZFighterStateTransition> GetCurrentTransitions() {
        return machine.GetChildrenTransitions(treePath.Count <= 0 ? -1 : treePath[treePath.Count - 1]);
    }

}