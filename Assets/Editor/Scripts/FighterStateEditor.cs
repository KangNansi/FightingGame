﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FightingGame
{
    public class FighterStateEditor {
        FighterState state;

        static Material lineMaterial;
        static void CreateLineMaterial()
        {
            if (!lineMaterial)
            {
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                lineMaterial = new Material(shader);
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                lineMaterial.SetInt("_ZWrite", 0);
            }
        }

        public FighterStateEditor(FighterState f)
        {
            state = f;
        }

	    public void OnGUI()
        {
            GUILayout.BeginVertical();

            state.name = EditorGUILayout.TextField(state.name);

            

            state.sprite = (Sprite)EditorGUILayout.ObjectField(state.sprite, typeof(Sprite), false);
            if (state.sprite)
            {
                Rect rect = EditorGUILayout.GetControlRect();
                rect.width = state.sprite.rect.width;
                rect.height = state.sprite.rect.height;
                GUILayout.Space(state.sprite.rect.height);
                GUI.DrawTextureWithTexCoords(rect, state.sprite.texture, GetSpriteRect(state.sprite));
            
                switch (Event.current.type)
                {

                    case EventType.Repaint:
                        foreach(HitBox h in state.hitboxes)
                        {
                            DrawHitbox(h, rect);
                        }
                        break;
                    default:
                        break;
                }

            }

            state.time = EditorGUILayout.Slider(state.time, 0, 1);
            //Hitboxes
            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Hitbox"))
            {
                state.hitboxes.Add(new HitBox(HitBox.Type.Attack, new Vector2(0,0), new Vector2(1,1)));
            }
            
            GUILayout.EndHorizontal();
            if (state.hitboxes.Count > 0)
            {
                foreach(HitBox h in state.hitboxes)
                {
                    HitboxGUI(h);
                }
            }

            GUILayout.EndVertical();
        }

        public static void DrawState(FighterState state, float h)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            if (state != null && state.sprite != null)
            {
                rect.width = state.sprite.rect.width;
                rect.height = h;
            }
                GUILayout.Space(h);
            if (state != null && state.sprite != null) {
                GUI.DrawTextureWithTexCoords(rect, state.sprite.texture, GetSpriteRect(state.sprite));
            }
        }

        static Rect GetSpriteRect(Sprite s)
        {
            return new Rect(s.textureRect.position.x / (float)s.texture.width,
                s.textureRect.position.y / (float)s.texture.height,
                s.textureRect.size.x / (float)s.texture.width,
                s.textureRect.size.y / (float)s.texture.height);
        }

        void DrawHitbox(HitBox hb, Rect position)
        {
            Vector2 size;
            size.x = hb._size.x * state.sprite.pixelsPerUnit;
            size.y = hb._size.y * state.sprite.pixelsPerUnit;
            Vector2 pos;
            pos.x = state.sprite.pivot.x + hb._position.x * state.sprite.pixelsPerUnit;
            pos.y = state.sprite.rect.height-state.sprite.pivot.y - hb._position.y * state.sprite.pixelsPerUnit;
            CreateLineMaterial();
            lineMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadPixelMatrix();
            GL.Begin(GL.QUADS);
            if(hb._type == HitBox.Type.Attack)
            {
                GL.Color(new Color(1,0,0,0.5f));
            }
            if (hb._type == HitBox.Type.Body)
            {
                GL.Color(new Color(0, 0, 1, 0.5f));
            }
            GL.Vertex3(position.x+ pos.x, position.y + pos.y, 0);
            GL.Vertex3(position.x + pos.x, position.y + pos.y - size.y, 0);
            GL.Vertex3(position.x + pos.x + size.x, position.y + pos.y - size.y, 0);
            GL.Vertex3(position.x + pos.x + size.x, position.y + pos.y, 0);
            GL.End();
            GL.PopMatrix();
        }

        void HitboxGUI(HitBox hb)
        {
            hb._type = (HitBox.Type)EditorGUILayout.EnumPopup("Type", hb._type);
            hb._position = EditorGUILayout.Vector2Field("Pos:", hb._position);
            hb._size = EditorGUILayout.Vector2Field("Size:", hb._size);
            if (GUILayout.Button("Remove"))
            {
                state.hitboxes.Remove(hb);
            }
        }
    }
}
