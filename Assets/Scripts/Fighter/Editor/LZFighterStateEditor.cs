using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZFight;
using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

public class LZFighterStateEditor : EditorWindow {

    LZFighterFrameEditor frameEditor = new LZFighterFrameEditor();

    LZFighterStateData data;
    Vector2 leftScroll = Vector2.zero;

    int selectedFrame = -1;

    private void Awake() {
        wantsMouseMove = true;
    }

    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line) {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is LZFighterStateData) {
            LZFighterStateEditor window = EditorWindow.GetWindow<LZFighterStateEditor>();
            window.data = (LZFighterStateData)obj;
            window.Show();
            //window.Init();
            return true;
        }
        return false; // we did not handle the open
    }

    public static void Open(LZFighterStateData data) {
        LZFighterStateEditor window = EditorWindow.GetWindow<LZFighterStateEditor>();
        window.data = data;
        window.Init();
        window.Show();
    }

    private void Init() {
        if(data.frames.Count > 0) {
            selectedFrame = 0;
            frameEditor.SetFrame(data.frames[0]);
            Repaint();
        }
    }

    private void OnGUI() {
        wantsMouseMove = true;
        Event ev = Event.current;
        frameEditor.SetState(data);
        Draw();
    }

    private void Draw() {
        GUILayout.BeginArea(new Rect(0, 0, 100, position.height));
        leftScroll = GUILayout.BeginScrollView(leftScroll, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);
        for(int i = 0; i < data.frames.Count; i++) {
            bool isSelected = (i == selectedFrame);
            if (FramePreviewButton(data.frames[i], isSelected, i)) {
                selectedFrame = i;
                frameEditor = new LZFighterFrameEditor();
                frameEditor.SetFrame(data.frames[i]);
            }
        }
        if(AddFrameButton()) {
            if (data.frames.Count > 0) {
                data.frames.Add(new LZFighterFrame(data.frames[data.frames.Count - 1]));
            }
            else {
                data.frames.Add(new LZFighterFrame());
            }
        }
        GUILayout.Label(selectedFrame.ToString());
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        frameEditor.OnGUI(new Rect(100,0,position.width-100, position.height));
    }

    private bool FramePreviewButton(LZFighterFrame frame, bool selected, int frameID) {
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUITools.NodeStyle, GUILayout.Height(75), GUILayout.Width(75));
        bool clicked = false;
        Event ev = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        GUITools.ButtonInfo info = GUIUtility.GetStateObject(typeof(GUITools.ButtonInfo), controlID) as GUITools.ButtonInfo;

        switch (ev.GetTypeForControl(controlID)) {
            case EventType.MouseUp:
                info.mouseDown = false;
                if (rect.Contains(ev.mousePosition)) {
                    clicked = true;
                }
                if (GUIUtility.hotControl == controlID) {
                    ev.Use();
                    GUIUtility.hotControl = 0;
                }
                break;
            case EventType.MouseDown:
                if (rect.Contains(ev.mousePosition)) {
                    info.mouseDown = true;
                    ev.Use();
                    GUIUtility.hotControl = controlID;
                    if (ev.clickCount == 2) {
                        EditorGUIUtility.ShowObjectPicker<Sprite>(frame.sprite, false, "", controlID);
                    }
                }
                break;

            case EventType.ExecuteCommand:
                if (ev.commandName == "ObjectSelectorUpdated" && selectedFrame == frameID) {
                    frame.sprite = (Sprite)EditorGUIUtility.GetObjectPickerObject();
                    ev.Use();
                }
                break;
            
            case EventType.Repaint:
                Color rectangle = new Color(0.6f, 0.6f, 0.6f);
                if (selected) {
                    rectangle = GUITools.darkCyan;
                }
                else if (info.mouseDown) {
                    rectangle = Color.gray;
                }
                rect = GUITools.PaddedContainer.padding.Remove(rect);
                EditorGUI.DrawRect(rect, rectangle);

                if(frame.sprite != null) {
                    GUI.DrawTextureWithTexCoords(rect, frame.sprite.texture, frame.sprite.GetSpriteRect());
                }
                break;
        }

        return clicked;
    }

    private bool AddFrameButton() {
        Rect rect = GUILayoutUtility.GetRect(GUIContent.none, GUITools.NodeStyle, GUILayout.Height(75), GUILayout.Width(75));
        bool clicked = false;
        Event ev = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        GUITools.ButtonInfo info = GUIUtility.GetStateObject(typeof(GUITools.ButtonInfo), controlID) as GUITools.ButtonInfo;

        switch (ev.GetTypeForControl(controlID)) {
            case EventType.MouseUp:
                info.mouseDown = false;
                if (rect.Contains(ev.mousePosition)) {
                    clicked = true;
                }
                if(GUIUtility.hotControl == controlID) {
                    ev.Use();
                    GUIUtility.hotControl = 0;
                }
                break;
            case EventType.MouseDown:
                if (rect.Contains(ev.mousePosition)) {
                    info.mouseDown = true;
                    ev.Use();
                    GUIUtility.hotControl = controlID;
                }
                break;
            case EventType.Repaint:
                if (info.mouseDown) {
                    GUI.color = Color.gray;
                }
                rect = GUITools.PaddedContainer.padding.Remove(rect);
                GUITools.NodeStyle.Draw(rect, GUIContent.none, controlID);
                GUITools.BigWhiteText.Draw(rect, new GUIContent("+"), controlID);

                GUI.color = Color.white;
                break;
        }

        return clicked;
    }

}
