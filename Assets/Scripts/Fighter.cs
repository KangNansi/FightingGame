using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{


    public class Fighter : MonoBehaviour {
        public int controllerNumber = 1;
        VirtualController controller = VirtualController.GetController(1);
        public float speed;
        public int currentState = 0;
        public GameObject opponent;
        bool Grounded = false;

        [SerializeField]
        public MoveSet moveSet = new MoveSet();
        [SerializeField]
        public List<Move> moves = new List<Move>();
        public int Stand = 0;
        public int Crouch = 0;

        bool running = false;

        Vector3 velocity = new Vector3();
        public float jumpStrength = 30;
        private bool jumped = false;
       

        // Use this for initialization
        void Start () {
            running = true;
            controller = VirtualController.GetController(controllerNumber);
	    }

        public string[] GetMoveList()
        {
            List<string> r = new List<string>();
            for(int i =0; i< moves.Count; i++)
            {
                r.Add(moves[i].name);
            }
            return r.ToArray();
        }
	
	    // Update is called once per frame
	    void Update () {
            if (!running) return;
            if (moves[currentState].Compute(Time.deltaTime))
            {
                SetMove(Stand);
            }
            float h = controller.GetHorizontal();
            float v = -controller.GetVertical();
            //clamp to 1
            if (h < -0.5) h = -1;
            else if (h > 0.5) h = 1;
            if (v < -0.5) v = -1;
            else if (v > 0.5) v = 1;
            else v = 0;
            Debug.Log(v);
            //Crouch
            if (v <= 0 && jumped) jumped = false;
            if (v < 0 && Grounded)
            {
                SetMove(Crouch);
            }
            else if (v > 0 && Grounded && !jumped)
            {
                Grounded = false;
                velocity.y = jumpStrength;
                jumped = true;
            }
            else
            {
                SetMove(Stand);
            }
            if (Grounded) velocity = Vector3.zero;
            //Movement
            if(currentState == Stand && Grounded) velocity += h * speed * Vector3.right;
            //Direction
            if (opponent)
            {
                if (opponent.transform.position.x < transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
                else transform.localScale = new Vector3(1, 1, 1);
            }
            //Attack
            Node node = moveSet.nodes.Find((Node n) => n.moveId == currentState);
            if (node != null)
            {
                foreach(Action a in node.actions)
                {
                    if (controller.GetKeyDown(a.input))
                    {
                        SetMove(a.state);
                    }
                }
            }

            //Apply gravity
            float g = FightManager.gravity;
            velocity += Vector3.down * g;
            transform.position += velocity * Time.deltaTime;
            if (transform.position.y < FightManager.groundHeight)
            {
                transform.position = new Vector3(transform.position.x, FightManager.groundHeight, 0);
                Grounded = true;
            }
	    }

        bool Standing()
        {
            if (currentState == Stand) return true;
            return false;
        }

        void SetMove(int newMove)
        {
            currentState = newMove;
            moves[currentState].Reset();
        }

        public void OnRenderObject()
        {
            moves[currentState].Draw(transform.localToWorldMatrix);
        }

        private void OnDrawGizmos()
        {
            OnRenderObject();
        }

        public static void Hit(Fighter a, Fighter b)
        {
            FighterState aframe = a.moves[a.currentState].GetFrame(), bframe = b.moves[b.currentState].GetFrame();
            List<HitBox> aattack = aframe.GetHitbox(HitBox.Type.Attack), battack = bframe.GetHitbox(HitBox.Type.Attack);
            List<HitBox> adef = aframe.GetHitbox(HitBox.Type.Body), bdef = bframe.GetHitbox(HitBox.Type.Body);
            foreach(HitBox h in aattack)
            {
                foreach(HitBox d in bdef)
                {
                    HitBox attack = new HitBox(h, a.transform);
                    HitBox def = new HitBox(d, b.transform);
                    Debug.Log("attack"+attack._position+" "+attack._size);
                    Debug.Log("defense"+def._position+" "+def._size);
                    if (attack.Hit(def))
                    {
                        //Attack link
                        Debug.Log("Hit!");
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
                        //Attack link

                    }
                }

            }
        }
    }
}
