﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class FighterObject : ScriptableObject {

        public GameObject controller;

        [SerializeField]
        public MoveSet moveSet = new MoveSet();
        [SerializeField]
        public List<Move> moves = new List<Move>();

        public enum Event
        {
            Teabag, FireBall
        };

        public GameObject fireBall = null;

        public delegate void EventDelegate(Event eventType);
        public event EventDelegate FightEvent;

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
        public int JumpStart = 0;
        public int JumpFall = 0;
        public int JumpRecovery = 0;

        public bool running = false;

        public Vector3 velocity = new Vector3();
        public float jumpStrength = 30;
        public float jumpSpeed = 2f;
        public bool jumped = false;

        public float lifeMax = 100f;
        public float guardMax = 100f;
        public float stunMax = 100f;
        public float parryTime = 0.5f;
        public float parryPerfectTime = 0.1f;
        public float stunDecrease = 20;
        public float guardDecrease = 20;
        float life = 0.0f;
        public float guard = 0.0f;
        public float stun = 0.0f;
        public float combo = 0.0f;
        float parryTimer = 0.0f;
        bool parried = false;
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

        public Jauge teabagCharge = new Jauge(0, 100);

        FighterState lastState = null;

        public void Init()
        {
            life = lifeMax;
            FightEvent += EventHandler;
        }

        public bool Standing()
        {
            if (currentState == Stand || currentState == Walk) return true;
            return false;
        }

        public bool Attacking()
        {
            return (currentState != Stand && currentState != Walk && currentState != Hit);
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
            stun += hb.stun;
            if(stun < stunMax)
            {
                SetMove(Hit);
            }
            else
            {
                FallDown();
                stun = 0.0f;
            }
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

        public void Parry()
        {
            if(parryTimer>4*parryTime || parried)
            {
                parryTimer = 0.0f;
                parried = false;
            }
        }

        public bool PerfectParrying()
        {
            return (parryTimer < parryPerfectTime && !Attacking() && !parried);
        }

        public bool Parrying()
        {
            return (parryTimer < parryTime && !Attacking() && !parried);
        }

        public bool ReceiveGuardDamage(float dmg)
        {
            guard += dmg;
            return guard > guardMax;
        }


        public bool CanBlock()
        {
            return guard < guardMax && Standing();
        }

        public void UpdateObject(float deltaTime)
        {
            FighterState state = GetFrame();
            if (state != lastState)
            {
                if (state.wwiseEvent && controller != null)
                {
                    AkSoundEngine.PostEvent(state.wwiseEventName, controller);
                }
                //Launch Events
                foreach(Event e in state.events)
                {
                    if (FightEvent != null)
                    {
                        Debug.Log("Invoking");
                        FightEvent.Invoke(e);
                    }
                }
            }

            lastState = state;
            parryTimer += deltaTime;
            guard -= guardDecrease * deltaTime;
            stun -= stunDecrease * deltaTime;
            if (guard < 0) guard = 0.0f;
            if (stun < 0) stun = 0.0f;
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

        void EventHandler(Event e)
        {
            switch (e)
            {
                case Event.Teabag:
                    teabagCharge += 20;
                    break;

                case Event.FireBall:
                    Vector3 pos = GetFrame().GetMiddleOf(HitBox.Type.Attack);
                    pos.x *= controller.transform.lossyScale.x;
                    pos.y *= controller.transform.lossyScale.y;
                    GameObject g = Instantiate<GameObject>(fireBall, controller.transform.position+pos, Quaternion.identity);
                    g.GetComponent<FireBall>().opponent = controller.GetComponent<FighterController>().opponent;
                    g.GetComponent<FireBall>().velocity *= controller.transform.localScale.x;
                    break;
            }
        }
    }
}
