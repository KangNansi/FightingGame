﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace FightingGame
{
    public class FighterObject : ScriptableObject {

        [SerializeField]
        public MoveSet moveSet = new MoveSet();
        [SerializeField]
        public List<Move> moves = new List<Move>();

        public float speed;
        public int currentState = 0;
        public bool drawHitbox = true;
        public bool Grounded = false;

        public int Stand = 0;
        public int Crouch = 0;
        public int Walk = 0;
        public int Hit = 0;
        public int Block = 0;
        public int Taunt = 0;
        public int Fall = 0;
        public int GetUp = 0;

        public bool running = false;

        public Vector3 velocity = new Vector3();
        public float jumpStrength = 30;
        public bool jumped = false;

        public float lifeMax = 100f;
        public float guardMax = 100f;
        public float stunMax = 100f;
        float life = 0.0f;
        float guard = 0.0f;
        float stun = 0.0f;
        float combo = 0.0f;
        public float Life
        {
            get
            {
                return life;
            }
        }
        public float Combo
        {
            get
            {
                return combo;
            }
        }

        public float fallTime = 1.0f;
        bool bFallen = false;
        float fallTimer = 0.0f;

        public void Init()
        {
            life = lifeMax;
        }

        public bool Standing()
        {
            if (currentState == Stand || currentState == Walk) return true;
            return false;
        }

        public string[] GetMoveList()
        {
            List<string> r = new List<string>();
            for (int i = 0; i < moves.Count; i++)
            {
                r.Add(moves[i].name);
            }
            return r.ToArray();
        }

        public void SetMove(int newMove)
        {
            currentState = newMove;
            moves[currentState].Reset();
        }

        public FighterState GetFrame()
        {
            return moves[currentState].GetFrame();
        }

        public HitBox GetAttackHitbox()
        {
            FighterState state = GetFrame();
            foreach(HitBox h in state.hitboxes)
            {
                if(h._type == HitBox.Type.Attack)
                {
                    return h;
                }
            }
            return null;
        } 

        public Move GetMove()
        {
            return moves[currentState];
        }

        public void Damage(HitBox hb)
        {
            combo += hb.dmg;
            combo = Mathf.Min(combo, life);
            guard += hb.guardDmg;
            stun += hb.stun;
            SetMove(Hit);
        }

        public void ConfirmHit()
        {
            life -= combo;
            combo = 0;
            if (life > 0)
                FallDown();
            else
                Die();
        }

        public void FallDown()
        {
            SetMove(Fall);
            fallTimer = 0.0f;
            bFallen = true;
        }

        public void Die()
        {
            SetMove(Fall);
        }

        public void UpdateObject(float deltaTime)
        {
            if (bFallen)
            {
                fallTimer += deltaTime;
                if (fallTimer >= fallTime)
                {
                    SetMove(GetUp);
                    bFallen = false;
                }
            }
        }
    }
}
