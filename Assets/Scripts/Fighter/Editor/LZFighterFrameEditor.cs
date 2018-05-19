using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZFight;
using UnityEngine;
using UnityEditor;
using FightingGame;

public class LZFighterFrameEditor {

    private int inspectorWidth = 300;
    LZFighterFrame frame;
    LZFighterStateData fighterState;
    float editorZoom = 1f;
    Vector2 centerOffset = Vector2.zero;
    HitBox selectedHitBox = null;
    private enum STATE {
        IDLE, RESIZE
    };
    private STATE state = STATE.IDLE;
    private MouseCursor cursor;

    bool left;
    bool up;
    bool right;
    bool down;
    bool dragging = false;

    public void SetFrame(LZFighterFrame frame) {
        this.frame = frame;
    }

    public void SetState(LZFighterStateData data) {
        this.fighterState = data;
    }

    public void OnGUI(Rect rect) {
        Rect editorRect = new Rect(Vector2.zero, new Vector2(rect.size.x - inspectorWidth, rect.size.y));
        Rect inspectorRect = new Rect(new Vector2(rect.size.x - inspectorWidth, 0), new Vector2(inspectorWidth, rect.size.y));
        GUILayout.BeginArea(rect);
        EditorGUI.DrawRect(editorRect, new Color(0.2f, 0.2f, 0.2f));

        DrawEditor(editorRect);
        DrawInspector(inspectorRect);
        GUILayout.EndArea();
        
        

    }

    private void DrawEditor(Rect rect) {
        GUILayout.BeginArea(rect);
        Event ev = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Keyboard);
        if (frame != null && frame.sprite != null) {
            Vector2 center = new Vector2(rect.x + rect.width / 2, rect.y + rect.height / 2) + centerOffset;
            Rect spriteRect = new Rect(center - (frame.sprite.rect.size / 2) * editorZoom, (frame.sprite.rect.size) * editorZoom);
            Vector2 pivot = center - new Vector2(0, (frame.sprite.rect.size.y) * editorZoom);
            Vector2 hitboxPoint = (ev.mousePosition - pivot) / editorZoom;

            ProcessHitboxEvents(selectedHitBox, center);

            switch (ev.GetTypeForControl(controlID)) {
                case EventType.KeyDown:
                    switch (ev.keyCode) {
                        case KeyCode.Delete:
                            if(selectedHitBox != null) {
                                frame.hitboxes.Remove(selectedHitBox);
                                selectedHitBox = null;
                                ev.Use();
                            }
                            break;
                        case KeyCode.F:
                            centerOffset = Vector2.zero;
                            ev.Use();
                            break;
                    }
                    break;

                case EventType.ScrollWheel:
                    editorZoom = Mathf.Clamp(editorZoom - ev.delta.y*0.1f, 0.1f, 100f);
                    ev.Use();
                    break;
                case EventType.MouseDown:
                    if(ev.button == 0) {
                        bool selected = false;
                        foreach(HitBox hb in frame.hitboxes) {
                            Rect hitRect = HitboxToGUI(hb, frame.sprite, center);
                            if (hitRect.Contains(ev.mousePosition)) {
                                selectedHitBox = hb;
                                selected = true;
                            }
                        }
                        if (!selected) {
                            selectedHitBox = null;
                        }
                        else {
                            frame.hitboxes.Remove(selectedHitBox);
                            frame.hitboxes.Insert(0, selectedHitBox);
                            dragging = true;
                        }
                    }
                    else if(ev.button == 1){
                        Vector2 point = HitboxGUIPoint(ev.mousePosition, frame.sprite, center);
                        frame.hitboxes.Add(new HitBox(HitBox.Type.Attack, point-new Vector2(0.1f, 0.1f), new Vector2(0.2f, 0.2f)));
                    }
                    ev.Use();
                    break;
                case EventType.MouseDrag:
                    centerOffset += ev.delta;
                    ev.Use();
                    break;

            }
        
            
            GUI.DrawTextureWithTexCoords(spriteRect, frame.sprite.texture, frame.sprite.GetSpriteRect());

            foreach(HitBox hb in frame.hitboxes) {
                DrawHitbox(hb, center);
            }

        }
        GUILayout.EndArea();
    }

    private void ProcessHitboxEvents(HitBox hitbox, Vector2 pivot) {
        if(hitbox == null) {
            return;
        }
        Rect hitboxRect = HitboxToGUI(hitbox, frame.sprite, pivot);
        Vector2 A = hitboxRect.position;
        Vector2 B = hitboxRect.position + new Vector2(hitboxRect.size.x, 0);
        Vector2 C = hitboxRect.position + hitboxRect.size;
        Vector2 D = hitboxRect.position + new Vector2(0, hitboxRect.size.y);
        float ratio = frame.sprite.pixelsPerUnit;
        int controlID = EditorGUIUtility.GetControlID(FocusType.Keyboard);
        Event ev = Event.current;
        Vector2 hitboxPoint = ev.mousePosition;
        
        switch (ev.GetTypeForControl(controlID)) {
            case EventType.MouseDown:
                if (hitboxRect.Contains(ev.mousePosition) || left || up || right || down) {
                    EditorGUIUtility.hotControl = controlID;
                    dragging = true;
                    ev.Use();
                }
                break;

            case EventType.MouseDrag:
                if(dragging) {
                    Vector2 delta = ev.delta / editorZoom;
                    delta /= ratio;
                    delta.y = -delta.y;
                    if (left) {
                        hitbox._position.x += delta.x;
                        hitbox._size.x -= delta.x;
                    }
                    if (down) {
                        hitbox._position.y += delta.y;
                        hitbox._size.y -= delta.y;
                    }
                    if (right) {
                        hitbox._size.x += delta.x;
                    }
                    if (up) {
                        hitbox._size.y += delta.y;
                    }
                    if(!left && !up && !right && !down) {
                        hitbox._position += delta;
                    }
                    ev.Use();
                }
                break;

            case EventType.MouseUp:
                if(EditorGUIUtility.hotControl == controlID) {
                    EditorGUIUtility.hotControl = 0;
                }
                break;

            case EventType.MouseMove:
                left = GUITools.SegmentPointDistance(A, D, ev.mousePosition) < 7;
                up = GUITools.SegmentPointDistance(A, B, ev.mousePosition) < 7;
                right = GUITools.SegmentPointDistance(B, C, ev.mousePosition) < 7;
                down = GUITools.SegmentPointDistance(C, D, ev.mousePosition) < 7;
                Debug.Log("Moved");
                if (up && left)
                    cursor = MouseCursor.ResizeUpLeft;
                else if (up && right)
                    cursor = MouseCursor.ResizeUpRight;
                else if (down && left)
                    cursor = MouseCursor.ResizeUpRight;
                else if (down && right)
                    cursor = MouseCursor.ResizeUpLeft;
                else if (left || right)
                    cursor = MouseCursor.ResizeHorizontal;
                else if(up || down)
                    cursor = MouseCursor.ResizeVertical;
                else
                    cursor = MouseCursor.Arrow;
                ev.Use();
                break;

            case EventType.Repaint:
                EditorGUIUtility.AddCursorRect(new Rect(0, 0, 10000, 10000), cursor);
                break;
        }
    }

    private void DrawHitbox(HitBox hitbox, Vector2 pivot) {

        Color boxColor = Color.white;
        switch (hitbox._type) {
            case HitBox.Type.Attack:
                boxColor = Color.red;
                break;
            case HitBox.Type.Body:
                boxColor = Color.blue;
                break;
        }
        boxColor.a = 0.30f;
        Rect boxRect = HitboxToGUI(hitbox, frame.sprite, pivot);

        

        EditorGUI.DrawRect(boxRect, boxColor);
        
        if (selectedHitBox == hitbox) {
            Handles.BeginGUI();
            Handles.DrawLine(boxRect.position, boxRect.position + new Vector2(boxRect.size.x, 0));
            Handles.DrawLine(boxRect.position, boxRect.position + new Vector2(0, boxRect.size.y));
            Handles.DrawLine(boxRect.position + boxRect.size, boxRect.position + new Vector2(boxRect.size.x, 0));
            Handles.DrawLine(boxRect.position + boxRect.size, boxRect.position + new Vector2(0, boxRect.size.y));
            Handles.EndGUI();
        }
    }

    private void DrawInspector(Rect rect) {
        GUILayout.BeginArea(rect);// new Rect(rect.size.x - inspectorWidth, 0, inspectorWidth, rect.size.y));
        EditorGUILayout.BeginVertical(GUITools.PaddedContainer);

        //State Property
        /*EditorGUILayout.LabelField("State Property");
        EditorGUILayout.BeginVertical("Box");
        
        EditorGUILayout.EndVertical();*/

        if (frame != null) {
            frame.sprite = (Sprite)EditorGUILayout.ObjectField(frame.sprite, typeof(Sprite), false);
            frame.time = EditorGUILayout.Slider(frame.time, 0, 1f);
            frame.velocity = EditorGUILayout.Vector2Field("Velocity", frame.velocity);
            frame.frameType = (LZFighterFrame.Type)EditorGUILayout.EnumPopup("Frame Type:", frame.frameType);
            if (selectedHitBox != null) {
                EditorGUILayout.LabelField("HitBox Property");
                EditorGUILayout.BeginVertical("Box");
                selectedHitBox._type = (HitBox.Type)EditorGUILayout.EnumPopup(selectedHitBox._type);
                selectedHitBox.dmg = EditorGUILayout.FloatField("Damage:", selectedHitBox.dmg);
                selectedHitBox.stun = EditorGUILayout.FloatField("Stun:", selectedHitBox.stun);
                selectedHitBox.guardDmg = EditorGUILayout.FloatField("GuardDmg:", selectedHitBox.guardDmg);
                EditorGUILayout.EndVertical();
            }
            GUITools.ScriptListField(frame.scripts, null);
        }
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private Rect HitboxToGUI(HitBox hb, Sprite sprite, Vector2 origin) {
        Rect result = new Rect();
        Vector2 pivot = origin + new Vector2(-sprite.rect.size.x, sprite.rect.size.y) / 2f * editorZoom;
        Vector2 position = new Vector2(hb._position.x, -hb._position.y - hb._size.y);
        Rect hitboxRect = new Rect(position * sprite.pixelsPerUnit, hb._size);
        hitboxRect.size *= sprite.pixelsPerUnit;
        hitboxRect.position *= editorZoom;
        hitboxRect.size *= editorZoom;
        Vector2 spritePivot = new Vector2(sprite.pivot.x, -sprite.pivot.y);
        result.position = pivot + spritePivot * editorZoom + hitboxRect.position;
        result.size = hitboxRect.size;
        return result;
    }

    private Vector2 HitboxGUIPoint(Vector2 position, Sprite sprite, Vector2 origin) {
        Vector2 pivot = origin + new Vector2(-sprite.rect.size.x, sprite.rect.size.y) / 2f * editorZoom;
        Vector2 pointGUI = position - pivot - sprite.pivot;
        pointGUI /= editorZoom;
        pointGUI /= sprite.pixelsPerUnit;
        pointGUI.y = -pointGUI.y;
        return pointGUI;
    }
}
