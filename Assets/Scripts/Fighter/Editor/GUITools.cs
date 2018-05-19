using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using LZFight;

public static class GUITools {
    // COLORS
    public static Color first = new Color(1, 0.5f, 0.5f);
    public static Color second = new Color(1, 0.65f, 0.5f);
    public static Color third = new Color(1, 0.8f, 0.5f);

    public static void DrawArrow(Vector2 position, Vector2 direction, float size = 4f) {
        Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction);
        Vector2 A = rotation * new Vector2(0, size);
        Vector2 B = rotation * Quaternion.Euler(0, 0, 120) * new Vector2(0, size);
        Vector2 C = rotation * Quaternion.Euler(0, 0, 240) * new Vector2(0, size);
        Handles.BeginGUI();
        Handles.DrawAAConvexPolygon(position + A, position + B, position + C);
        Handles.EndGUI();
    }

    private static GUIStyle nodeStyle = null;
    public static GUIStyle NodeStyle{
        get {
            if(nodeStyle == null){
                nodeStyle = new GUIStyle(GUI.skin.GetStyle("flow node 1"));
                nodeStyle.alignment = TextAnchor.MiddleCenter;
                nodeStyle.padding = new RectOffset(3, 3, 3, 3);
                nodeStyle.margin = new RectOffset(5, 5, 5, 5);
                nodeStyle.onHover = GUI.skin.GetStyle("flow node 1 on").normal;
            }
            return nodeStyle;
        }
    }

    private static GUIStyle nodeStyleOn = null;
    public static GUIStyle NodeStyleOn {
        get {
            if (nodeStyleOn == null) {
                nodeStyleOn = new GUIStyle(GUI.skin.GetStyle("flow node 1 on"));
                nodeStyleOn.alignment = TextAnchor.MiddleCenter;
                nodeStyleOn.padding = new RectOffset(3, 3, 3, 3);
                nodeStyleOn.margin = new RectOffset(5, 5, 5, 5);
            }
            return nodeStyleOn;
        }
    }

    public static GUIStyle GetNodeStyle(int id, bool on) {
        GUIStyle style = new GUIStyle((GUIStyle)("flow node " + id + (on?" on":"")));
        style.alignment = TextAnchor.MiddleCenter;
        style.padding = new RectOffset(3, 3, 3, 3);
        style.margin = new RectOffset(5, 5, 5, 5);
        return style;
    }

    private static GUIStyle paddedNodeStyle = null;
    public static GUIStyle PaddedNodeStyle {
        get {
            if(paddedNodeStyle == null) {
                paddedNodeStyle = new GUIStyle(NodeStyle);
                paddedNodeStyle.margin = new RectOffset(15, 15, 15, 15);
                paddedNodeStyle.padding = new RectOffset(15, 15, 15, 15);
            }
            return paddedNodeStyle;
        }
    }

    private static GUIStyle greyBlock = null;
    public static GUIStyle GreyBlock {
        get {
            if (greyBlock == null) {

                greyBlock = new GUIStyle(GUI.skin.box);
                //greyBlock.normal.background = EditorGUIUtility.FindTexture("tree_node");
                //greyBlock.onNormal.background = greyBlock.normal.background;
                greyBlock.padding = new RectOffset(3, 3, 3, 3);
                greyBlock.margin = new RectOffset(2, 2, 2, 2);
            }
            return greyBlock;
        }
    }

    

    private static GUIStyle paddedContainer = null;
    public static GUIStyle PaddedContainer {
        get {
            if (paddedContainer == null) {
                paddedContainer = new GUIStyle();
                paddedContainer.padding = new RectOffset(3, 3, 3, 3);
            }
            return paddedContainer;
        }
    }

    private static GUIStyle middleText = null;
    public static GUIStyle MiddleText {
        get {
            if (middleText == null) {
                middleText = new GUIStyle();
                middleText.padding = new RectOffset(3, 3, 3, 3);
                middleText.alignment = TextAnchor.MiddleCenter;
                middleText.normal.textColor = Color.white;
                middleText.fontSize = 16;
                middleText.fontStyle = FontStyle.Bold;
            }
            return middleText;
        }
    }

    private static GUIStyle bigWhiteText = null;
    public static GUIStyle BigWhiteText {
        get {
            if (bigWhiteText == null) {
                bigWhiteText = new GUIStyle();
                bigWhiteText.alignment = TextAnchor.MiddleCenter;
                bigWhiteText.normal.textColor = Color.white;
                bigWhiteText.fontSize = 46;
                bigWhiteText.fontStyle = FontStyle.Bold;
            }
            return bigWhiteText;
        }
    }

    private static GUIStyle miniCenteredButton = null;
    public static GUIStyle MiniCenteredButton {
        get {
            if (miniCenteredButton == null) {
                miniCenteredButton = new GUIStyle(EditorStyles.miniButton);
                miniCenteredButton.alignment = TextAnchor.MiddleCenter;
                miniCenteredButton.margin = new RectOffset(5, 5, 5, 5);
            }
            return miniCenteredButton;
        }
    }

    public class ButtonInfo {
        public bool mouseDown = false;
    }

    public static Color darkCyan = new Color(0.25f, 0.5f, 0.9f);

    // MATHS

    public static float SegmentPointDistance(Vector2 v, Vector2 w, Vector2 p) {
        float lineLength = Vector2.SqrMagnitude(v - w);
        // Return minimum distance between line segment vw and point p
        float l2 = Vector2.SqrMagnitude(v - w);  // i.e. |w-v|^2 -  avoid a sqrt
        if (l2 == 0.0)
            return Vector2.Distance(p, v);   // v == w case
                                     // Consider the line extending the segment, parameterized as v + t (w - v).
                                     // We find projection of point p onto the line. 
                                     // It falls where t = [(p-v) . (w-v)] / |w-v|^2
                                     // We clamp t from [0,1] to handle points outside the segment vw.
        float t = Mathf.Max(0, Mathf.Min(1, Vector2.Dot(p - v, w - v) / l2));
        Vector2 projection = v + t * (w - v);  // Projection falls on the segment
        return Vector2.Distance(p, projection);
    }

    public static Vector2 Rotated(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        Vector2 r = new Vector2();
        r.x = (cos * tx) - (sin * ty);
        r.y = (sin * tx) + (cos * ty);
        return r;
    }

    public static void ListField<T>(List<T> list, System.Action<int> onElement = null, System.Action<T> onAdd = null) {
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < list.Count; i++) {
            EditorGUILayout.BeginHorizontal(GreyBlock);
            if(onElement != null) {
                onElement.Invoke(i);
            }
            EditorGUILayout.BeginVertical(GUILayout.Width(25), GUILayout.Height(25), GUILayout.ExpandHeight(false));
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("x", MiniCenteredButton)) {
                list.RemoveAt(i);
                i--;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+")) {
            T toAdd = default(T);
            if(onAdd != null) onAdd.Invoke(toAdd);
            list.Add(toAdd);
        }
        EditorGUILayout.EndVertical();
    }

    public static void ScriptListField(List<MiniScript> scripts, LZFighter fighter) {
        ListField(scripts, (i) => {
            EditorGUILayout.BeginVertical(NodeStyle);
            scripts[i] = (MiniScript)EditorGUILayout.ObjectField(scripts[i], typeof(MiniScript), false);
            if (scripts[i] != null) {
                scripts[i].Initialize(fighter);
                Editor scriptEditor = Editor.CreateEditor(scripts[i]);
                scriptEditor.OnInspectorGUI();
            }
            EditorGUILayout.EndVertical();
        });
    }

    public static void DrawGrid(Rect rect, Vector2 offset, float gridSpacing, float gridOpacity, Color gridColor) {
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

    public static bool NoField(string label,bool value) {
        GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
        if (value) {
            style.normal.textColor = Color.black;
            GUI.color = new Color(0.7f, 0.7f, 1);
        }
        else {
            style.normal.textColor = new Color(0, 0, 0, 0.5f);
            GUI.color = new Color(1, 1, 1, 0.5f);
        }
        EditorGUILayout.LabelField(label, style, GUILayout.Width(20));
        GUI.color = Color.white;
        Rect rect = GUILayoutUtility.GetLastRect();
        int ControlID = EditorGUIUtility.GetControlID(FocusType.Keyboard);
        switch (Event.current.GetTypeForControl(ControlID)) {
            case EventType.MouseDown:
                if (rect.Contains(Event.current.mousePosition)) {
                    value = !value;
                    Event.current.Use();
                }
                break;
        }
        
        return value;
    }

}

