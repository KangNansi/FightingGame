﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [System.Serializable]
    public class HitBox {
        public Vector2 _position;
        public Vector2 _size;

        static Material debugMat;

        public enum Type
        {
            Attack,
            Body
        }
        [SerializeField]
        public Type _type;
        public Type _Type {
            set
            {
                _type = value;
                SetColor();
            }
        }
        [SerializeField]
        Color color;
        [SerializeField]
        public float dmg = 1.0f;
        public float stun = 1.0f;
        public float guardDmg = 1.0f;

        public HitBox(Type type, Vector2 position, Vector2 size)
        {
            _type = type;
            _position = position;
            _size = size;
            SetColor();
        }

        public HitBox(HitBox h)
        {
            _type = h._type;
            _size = h._size;
            _position = h._position;
            dmg = h.dmg;
            stun = h.stun;
            guardDmg = h.guardDmg;
        }

        public HitBox(HitBox h, Transform t)
        {
            _type = h._type;
            _size.x = h._size.x*Mathf.Abs(t.lossyScale.x);
            _size.y = h._size.y * Mathf.Abs(t.lossyScale.y);
            _position = h._position;
            _position.x *= t.lossyScale.x;
            _position.y *= t.lossyScale.y;
            if (t.lossyScale.x < 0)
                _position.x -= _size.x;
            if (t.lossyScale.y < 0)
                _position.x -= _size.x;
             _position += (Vector2)t.position;
            dmg = h.dmg;
            stun = h.stun;
            guardDmg = h.guardDmg;
        }

        protected void SetColor()
        {
            switch(_type){
                case Type.Attack: color = new Color(1, 0, 0, 0.5f); break;
                case Type.Body: color = new Color(0, 0, 1, 0.5f); break;
                default: break;
            }
        }

        public bool Hit(HitBox t)
        {
            if (t._position.x < _position.x + _size.x && t._position.x + t._size.x > _position.x
                && t._position.y < _position.y + _size.y && t._position.y + t._size.y > _position.y)
                return true;
            return false;
        }

        static HitBox Invert(HitBox hb)
        {
            HitBox result = new HitBox(hb);
            result.Invert();
            return result;
        }

        public void Invert()
        {
            _position.x += _size.x / 2f;
            _position.x *= -1;
            _position.x -= _size.x / 2f;
        }

        private static void CreateMaterial()
        {
            if (!debugMat)
            {
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                debugMat = new Material(shader);
                debugMat.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                debugMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                debugMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                debugMat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                debugMat.SetInt("_ZWrite", 0);
            }
        }

        public static Material GetDebugMaterial()
        {
            CreateMaterial();
            return debugMat;
        }

        public void Draw(Matrix4x4 p)
        {
            CreateMaterial();
            GL.PushMatrix();
            debugMat.SetPass(0);
            GL.MultMatrix(p);
            GL.Begin(GL.QUADS);
            if (_type == Type.Attack)
                GL.Color(new Color(1,0,0,0.4f));
            else
                GL.Color(new Color(0, 0, 1, 0.4f));
            GL.Vertex(_position);
            GL.Vertex3(_position.x + _size.x, _position.y, 0);
            GL.Vertex3(_position.x + _size.x, _position.y + _size.y, 0);
            GL.Vertex3(_position.x, _position.y + _size.y, 0);

            GL.End();
            GL.PopMatrix();
        }

        public bool IsPointIn(Vector2 position) {
            return _position.x < position.x && _position.y < position.y && _position.x + _size.x > position.x && _position.y + _size.y > position.y;
        }

        public Vector3 GetCenter() {
            return _position + _size / 2f;
        }
    }
}
