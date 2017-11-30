﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FighterController : MonoBehaviour {
        public FighterObject fighterObject;
        FighterObject fighter;
        public FighterObject Fighter
        {
            get
            {
                return fighter;
            }
        }
        public bool drawHitbox = true;
        //public int controllerNumber = 1;
        public VirtualController controller;
        public FighterController opponent;
        public float ComboStrength
        {
            get
            {
                return (fighter.Combo/(float)fighter.lifeMax);
            }
        }
        public float Life
        {
            get
            {
                return (fighter.Life/(float)fighter.lifeMax);
            }
        }

        public float hitFreezeTime = 0.1f;



        Vector2 sens = new Vector2();

        public GameObject hit;
        public GameObject hitBlock;
        public GameObject confirmSuccess;
        public GameObject confirmBigSuccess;
        public GameObject parry;
        public GameObject parryPerfect;
        public GameObject guardBreak;

        Vector3 addedVelocity = new Vector3(0, 0, 0);
        bool blocking = false;
        bool blocked = false;

        float minDistance = 1.5f;

        public void Reset()
        {
            fighter = Instantiate(fighterObject);
            fighter.running = true;
            fighter.Init();
        }

        void Start () {
            fighter = Instantiate(fighterObject);
            fighter.running = true;
            fighter.Init();
            controller = Instantiate(controller);
        }

        // Update is called once per frame
        void Update () {
            if (!fighter.running) return;
            ///Block
            if (controller.GetBlockDown())
                blocking = true;
            else if (controller.GetBlockUp())
                blocking = false;
            if (!blocking)
                blocked = false;
                
            float h = controller.GetHorizontal();
            float v = -controller.GetVertical();
            float deltaT = Time.deltaTime*FightManager.timeModifier;
            float dT = deltaT;
            fighter.UpdateObject(deltaT);
            if (fighter.currentState == fighter.Walk && h<0.0f)
            {
                dT *= -1;
            }
            if (fighter.GetMove().Compute(dT))
            {
                fighter.SetMove(fighter.GetMove().defaultNext);
            }
            //clamp to 1
            if (h < -0.5) h = -1;
            else if (h > 0.5) h = 1;
            if (v < -0.5) v = -1;
            else if (v > 0.5) v = 1;
            else v = 0;
            //Crouch
            if (v <= 0 && fighter.jumped) fighter.jumped = false;
            if (v < 0 && fighter.Grounded && fighter.Standing())
            {
                //fighter.SetMove(fighter.Crouch);
            }
            else if (v > 0 && fighter.Grounded && !fighter.jumped)
            {
                fighter.Grounded = false;
                fighter.velocity.y = fighter.jumpStrength;
                fighter.jumped = true;
            }
            else
            {
                //SetMove(Stand);
            }
            if (fighter.Grounded) fighter.velocity = Vector3.zero;
            //Movement
            if ((fighter.currentState == fighter.Stand || fighter.currentState == fighter.Walk) && fighter.Grounded)
            {
                float modif = 1.0f;
                if ((h < 0.0f && sens.x > 0.0f) || (h > 0.0f && sens.x < 0.0f))
                    modif = 0.7f;
                fighter.velocity += h * fighter.speed * modif * Vector3.right;
                if (fighter.currentState != fighter.Walk && h != 0)
                    fighter.SetMove(fighter.Walk);
                if (fighter.currentState == fighter.Walk && h == 0)
                    fighter.SetMove(fighter.Stand);
            }
            //Direction
            if (opponent)
            {
                if (opponent.transform.position.x < transform.position.x) sens = new Vector3(-1, 1, 1);
                else sens = new Vector3(1, 1, 1);
                transform.localScale = sens;
                controller.sens = sens.x;
            }
            //Attack
            int curState = fighter.currentState;
            if (curState == fighter.Walk) curState = fighter.Stand;
            Node node = fighter.moveSet.nodes.Find((Node n) => n.moveId == curState);
            if (node != null)
            {
                foreach (Action a in node.actions)
                {
                    if (controller.GetKeyDown(a.input) && fighter.GetFrame().frameType == FighterState.Type.RecoveryFrame)
                    {
                        fighter.SetMove(a.state);
                    }
                }
            }

            //Parry
            if (controller.GetBlockDown())
            {
                fighter.Parry();
            }

            //Confirm
            if(curState == fighter.Taunt && opponent.ComboStrength > 0)
            {
                HitBox hb = fighter.GetAttackHitbox();
                if (hb != null)
                {
                    if (opponent.ComboStrength < 0.3)
                    {
                        particleLaunch(confirmSuccess, transform.position+new Vector3((hb._position.x + hb._size.x / 2f)*sens.x, (hb._position.y + hb._size.y / 2f), 0));
                    }
                    else
                    {
                        particleLaunch(confirmBigSuccess, transform.position + new Vector3((hb._position.x + hb._size.x / 2f) * sens.x, (hb._position.y + hb._size.y / 2f), 0));
                    }
                    opponent.Confirm();
                }
            }

            //Apply gravity
            float g = FightManager.gravity;
            fighter.velocity += Vector3.down * g;
            fighter.velocity += ((Vector3)fighter.GetMove().GetVelocity()*sens.x);
            fighter.velocity += addedVelocity;
            transform.position += fighter.velocity * deltaT;

            //Players pushing each other
            float dist = (transform.position - opponent.transform.position).magnitude;
            if (dist<minDistance)
            {
                float displace = minDistance - dist;
                opponent.transform.position += new Vector3(sens.x * displace, 0);
            }

            if (transform.position.y < FightManager.groundHeight)
            {
                transform.position = new Vector3(transform.position.x, FightManager.groundHeight, 0);
                fighter.Grounded = true;
            }
        }

        public static void Hit(FighterController a, FighterController b)
        {
            FighterState aframe = a.fighter.GetFrame(), bframe = b.fighter.GetFrame();
            List<HitBox> aattack = aframe.GetHitbox(HitBox.Type.Attack), battack = bframe.GetHitbox(HitBox.Type.Attack);
            List<HitBox> adef = aframe.GetHitbox(HitBox.Type.Body), bdef = bframe.GetHitbox(HitBox.Type.Body);
            if(!aframe.Computed)
            {
                foreach (HitBox h in aattack)
                {
                    foreach (HitBox d in bdef)
                    {
                        HitBox attack = new HitBox(h, a.transform);
                        HitBox def = new HitBox(d, b.transform);
                        if (attack.Hit(def))
                        {
                            b.Hit(0, attack, a);
                        }
                    }

                }
                if (aattack.Count > 0)
                {
                    b.Block();
                }
            }

            if(!bframe.Computed)
            {
                foreach (HitBox h in battack)
                {
                    foreach (HitBox d in adef)
                    {
                        HitBox attack = new HitBox(h, b.transform);
                        HitBox def = new HitBox(d, a.transform);
                        if (attack.Hit(def))
                        {
                            a.Hit(0, attack, b);
                        }
                    }

                }
                if (battack.Count > 0)
                {
                    a.Block();
                }
            }


            aframe.Consume();
            bframe.Consume();
        }

        void Block()
        {
            if (controller.GetKeyDown(VirtualController.Keys.Block) && fighter.Standing())
            {
                if (!fighter.Parrying())
                {
                    fighter.SetMove(fighter.Block);
                }
            }
        }

        void Hit(int dmg, HitBox hitting, FighterController opponent)
        {
            if (fighter.Parrying()) //Perfect Parry
            {
                particleLaunch(parry, (Vector3)(transform.position + Vector3.up * 2f + new Vector3(sens.x, 0, 0)));
                float t = FightManager.timeModifier;
                FightManager.timeModifier = 0.0f;
                StartCoroutine(freezeTime(hitFreezeTime, t));
                return;
            }
            if (controller.GetKeyDown(VirtualController.Keys.Block) && fighter.Standing())//(controller.GetHorizontal() < -0.3f && sens.x > 0.1f) || (controller.GetHorizontal() > 0.3f && sens.x < -0.1f))
            {
                particleLaunch(hitBlock, (Vector3)(transform.position + Vector3.up*2f+new Vector3(sens.x,0,0)));
                StartCoroutine(blockPush());
                blocked = true;
            }
            else
            {
                particleLaunch(hit, (Vector3)(hitting._position + hitting._size/2f));
                float t = FightManager.timeModifier;
                FightManager.timeModifier = 0.0f;
                StartCoroutine(freezeTime(hitFreezeTime, t));
                fighter.Damage(hitting);
                StartCoroutine(blockPush());
            }
        }

        public void Confirm()
        {
            fighter.ConfirmHit();
        }

        public void OnRenderObject()
        {
            GetComponent<SpriteRenderer>().sprite = fighter.GetFrame().sprite;
            if (drawHitbox)
                fighter.GetMove().Draw(transform.localToWorldMatrix);
        }

        private void OnDrawGizmos()
        {
            GetComponent<SpriteRenderer>().sprite = fighterObject.GetFrame().sprite;
            if (drawHitbox)
                fighterObject.GetMove().Draw(transform.localToWorldMatrix);
        }

        IEnumerator blockPush()
        {
            float s = sens.x;
            addedVelocity += new Vector3(-s * 5, 0, 0);
            yield return new WaitForSeconds(0.3f);
            addedVelocity -= new Vector3(-s * 5, 0, 0);
        }

        IEnumerator freezeTime(float duration, float value)
        {
            yield return new WaitForSeconds(duration);
            FightManager.timeModifier = FightManager.defaultTimeModifier;
        }

        void particleLaunch(GameObject p, Vector3 pos)
        {
            GameObject s = Instantiate(p);
            s.transform.position = pos;
            s.transform.localScale = new Vector3(s.transform.localScale.x*sens.x, s.transform.localScale.y, s.transform.localScale.z);
            foreach(Transform g in s.transform)
            {
                g.localScale = new Vector3(g.localScale.x * sens.x, g.localScale.y, g.localScale.z);
            }
            StartCoroutine(launchParticle(s));
        }

        IEnumerator launchParticle(GameObject p)
        {
            yield return new WaitForSeconds(1f);
            Destroy(p);
        }
    }
}
