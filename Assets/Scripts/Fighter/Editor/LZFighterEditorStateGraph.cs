using LZFight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
/*
public class LZFighterEditorStateGraph {

    private bool dragging = false;
    private enum STATE {
        IDLE, MAKING_TRANSITION
    }
    private STATE state = STATE.IDLE;
    private LZFighterState transitionStart = null;

    private int selectedID = -1;
    private enum SELECTED_TYPE {
        STATE, TRANSITION
    }
    private SELECTED_TYPE selectedType = SELECTED_TYPE.STATE;


    public Vector2 graphOffset;
    private LZFighter fighter;

    private LZFighterEditorInspector inspector;

    public void SetFighter(LZFighter fighter) {
        this.fighter = fighter;
    }

    public object GetSelected() {
        if(selectedID < 0) {
            return null;
        }
        if(selectedType == SELECTED_TYPE.STATE) {
            return LZFighterEditor.Instance.fighter.states[selectedID];
        }
        if(selectedType == SELECTED_TYPE.TRANSITION) {
            return LZFighterEditor.Instance.fighter.transitions[selectedID];
        }
        return null;
    }

    public void DrawGraph(Rect rect) {
        GUILayout.BeginArea(rect, GUI.skin.box);
        EditorGUI.DrawRect(new Rect(Vector2.zero, rect.size), new Color(0.3f, 0.3f, 0.3f));
        DrawGrid(rect, graphOffset, 5f, 0.2f, Color.gray * 0.5f);
        DrawGrid(rect, graphOffset, 20f, 0.5f, Color.gray);

        //Draw Transition
        Handles.BeginGUI();
        foreach (LZFighterState state in fighter.states) {
            foreach(int tID in state.transitionID) {
                LZFighterState end = fighter.GetTransitionTarget(tID);
                Vector2 startP = state.nodeRect.center;
                Vector2 endP = end.nodeRect.center;
                Vector2 dir = endP - startP;
                Vector2 rotatedDir = dir.normalized.Rotated(90);
                startP += rotatedDir * 7;
                endP += rotatedDir * 7;
                if(selectedType == SELECTED_TYPE.TRANSITION && selectedID == tID) {
                    Handles.color = Color.cyan;
                }
                else {
                    Handles.color = Color.white;
                }
                Handles.DrawLine(startP, endP);
                GUITools.DrawArrow(startP + dir / 2f + dir.normalized*5f, dir.normalized);
                Handles.color = Color.white;
            }
        }
        if(state == STATE.MAKING_TRANSITION) {
            Handles.DrawLine(transitionStart.nodeRect.position + transitionStart.nodeRect.size/2f, Event.current.mousePosition);
        }
        Handles.EndGUI();

        //Draw nodes
        foreach (LZFighterState state in fighter.states) {
            DrawNode(state);
        }

        if (selectedID >= 0) {
            if(inspector == null) {
                inspector = new LZFighterEditorInspector();
            }
            inspector.fighter = fighter;
            inspector.OnInspectorGUI(new Rect(rect.size.x - 300, 0, 300, rect.size.y), this);
        }

        ProcessEvents();

        

        GUILayout.EndArea();
    }

    private void DrawGrid(Rect rect, Vector2 offset, float gridSpacing, float gridOpacity, Color gridColor) {
        int widthDivs = Mathf.CeilToInt(rect.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(rect.height / gridSpacing);


        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        //offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++) {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, rect.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++) {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(rect.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    public void ProcessEvents() {
        Event ev = Event.current;
        switch (ev.type) {
            case EventType.MouseDown:
                if(ev.button == 0) {
                    //Select Transition
                    foreach(LZFighterState state in fighter.states) {
                        for(int i = 0; i < state.transitionID.Count; i++) {
                            Vector2 A = state.nodeRect.center;
                            Vector2 B = fighter.GetTransitionTarget(state.transitionID[i]).nodeRect.center;
                            Vector2 dirMod = (B - A).normalized.Rotated(90);
                            A += dirMod * 7;
                            B += dirMod * 7;
                            if (GUITools.SegmentPointDistance(A, B, ev.mousePosition) < 7) {
                                selectedType = SELECTED_TYPE.TRANSITION;
                                selectedID = state.transitionID[i];
                                ev.Use();
                                return;
                            }
                        }
                    }

                    foreach(LZFighterState state in fighter.states) {
                        state.isDragged = true;
                    }
                    selectedID = -1;
                    dragging = true;
                    LZFighterEditor.RepaintWindow();
                }
                if(ev.button == 1) {
                    ProcessContextMenu(ev.mousePosition, fighter);
                }
                break;

            case EventType.MouseUp:
                foreach (LZFighterState state in fighter.states) {
                    state.isDragged = false;
                }
                EndTransition();
                dragging = false;
                break;

            case EventType.MouseDrag:
                foreach (LZFighterState state in fighter.states) {
                    if (state.isDragged) {
                        state.nodeRect.position += ev.delta;
                    }
                }
                if (dragging) {
                    graphOffset += ev.delta;
                }
                ev.Use();
                break;
        }
    }
    

    private void ProcessContextMenu(Vector2 mousePosition, LZFighter fighter) {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition, fighter));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition, LZFighter fighter) {
        /*string path = AssetDatabase.GetAssetPath(fighter);
        string filename = Path.GetFileNameWithoutExtension(path);
        string directory = Path.GetDirectoryName(path);
        if (!AssetDatabase.IsValidFolder(directory + "/" + filename)) {
            AssetDatabase.CreateFolder(directory, filename);
        }
        LZFighterState newState = new LZFighterState(fighter.states.Count, fighter.states.Count.ToString());
        newState.nodeRect.position = mousePosition;
        fighter.states.Add(newState);
        /*AssetDatabase.CreateAsset(newState, directory + "/" + filename + "/" + fighter.states.Count + ".asset");
        Debug.Log(Path.GetDirectoryName(path));
    }

    public void DrawNode(LZFighterState state) {
        GUILayout.BeginArea(state.nodeRect);
        GUIStyle style;
        if (state == GetSelected() as LZFighterState) {
            style = GUITools.NodeStyleOn;
        }
        else {
            style = GUITools.NodeStyle;
        }
        GUI.SetNextControlName("Node");
        using (var h = new EditorGUILayout.HorizontalScope(GUITools.PaddedContainer)) {
            GUILayout.BeginVertical(style);
            GUILayout.Label(state.stateName, GUITools.MiddleText);
            Vector2 size = GUITools.MiddleText.CalcSize(new GUIContent(state.stateName));
            GUILayout.EndVertical();
            Rect vRect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint) {
                state.nodeRect.height = h.rect.height;
                state.nodeRect.width = Mathf.Max(150, size.x+25);
            }
        }


        ProcessEvents(state);
        GUILayout.EndArea();
    }

    public void ProcessEvents(LZFighterState fighterState) {
        Event ev = Event.current;
        Rect rect = new Rect(Vector2.zero, fighterState.nodeRect.size);
        switch (ev.type) {
            case EventType.MouseDown:
                if (ev.button == 0 && rect.Contains(ev.mousePosition)) {
                    fighterState.isDragged = true;
                    selectedType = SELECTED_TYPE.STATE;
                    selectedID = fighterState.ID;
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
                    Debug.Log("Making transition between " + transitionStart.ID + " and " + fighterState.ID);
                    EndTransition(fighterState);
                    ev.Use();
                }
                break;
        }
    }

    private void BeginTransition(LZFighterState start) {
        transitionStart = start;
        state = STATE.MAKING_TRANSITION;
    }

    private void EndTransition(LZFighterState end) {
        state = STATE.IDLE;
        
        LZFighterEditor.Instance.fighter.AddTransition(transitionStart.ID, end.ID);
        LZFighterEditor.RepaintWindow();
    }

    private void EndTransition() {
        state = STATE.IDLE;
        LZFighterEditor.RepaintWindow();
    }

}

    */
