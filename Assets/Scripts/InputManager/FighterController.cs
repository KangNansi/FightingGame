using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FighterController : MonoBehaviour {
        public FighterObject fighter;
        public bool drawHitbox = true;
        public int controllerNumber = 1;
        VirtualController controller = VirtualController.GetController(1);
        public GameObject opponent;
        public float life = 100;

        void Start () {
            fighter = Instantiate(fighter);
            fighter.running = true;
            controller = VirtualController.GetController(controllerNumber);
        }

        // Update is called once per frame
        void Update () {
            if (!fighter.running) return;
            if (fighter.GetMove().Compute(Time.deltaTime))
            {
                fighter.SetMove(fighter.Stand);
            }
            float h = controller.GetHorizontal();
            float v = -controller.GetVertical();
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
                fighter.SetMove(fighter.Crouch);
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
                fighter.velocity += h * fighter.speed * Vector3.right;
                if (fighter.currentState != fighter.Walk && h != 0)
                    fighter.SetMove(fighter.Walk);
                if (fighter.currentState == fighter.Walk && h == 0)
                    fighter.SetMove(fighter.Stand);
            }
            //Direction
            if (opponent)
            {
                if (opponent.transform.position.x < transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
                else transform.localScale = new Vector3(1, 1, 1);
            }
            //Attack
            int curState = fighter.currentState;
            if (curState == fighter.Walk) curState = fighter.Stand;
            Node node = fighter.moveSet.nodes.Find((Node n) => n.moveId == curState);
            if (node != null)
            {
                foreach (Action a in node.actions)
                {
                    if (controller.GetKeyDown(a.input))
                    {
                        fighter.SetMove(a.state);
                    }
                }
            }

            //Apply gravity
            float g = FightManager.gravity;
            fighter.velocity += Vector3.down * g;
            transform.position += fighter.velocity * Time.deltaTime;
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
            foreach (HitBox h in aattack)
            {
                foreach (HitBox d in bdef)
                {
                    HitBox attack = new HitBox(h, a.transform);
                    HitBox def = new HitBox(d, b.transform);
                    if (attack.Hit(def))
                    {
                        b.Hit(0);
                    }
                }

            }

            foreach (HitBox h in battack)
            {
                foreach (HitBox d in adef)
                {
                    HitBox attack = new HitBox(h, b.transform);
                    HitBox def = new HitBox(d, a.transform);
                    if (attack.Hit(def))
                    {
                        a.Hit(0);
                    }
                }

            }
        }

        void Hit(int dmg)
        {
            Debug.Log("Hit!");
        }

        public void OnRenderObject()
        {
            GetComponent<SpriteRenderer>().sprite = fighter.GetFrame().sprite;
            if (drawHitbox)
                fighter.GetMove().Draw(transform.localToWorldMatrix);
        }

        private void OnDrawGizmos()
        {
            OnRenderObject();
        }
    }
}
