using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [ExecuteInEditMode]
    public class Fighter : MonoBehaviour {
        public float speed;
        [SerializeField]
        public List<Move> moves = new List<Move>();
        public int currentState = 0;
        public int Stand = 0;
        public GameObject opponent;

        public MoveSet moveSet;
        private bool running = false;

        // Use this for initialization
        void Start () {
            running = true;
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
            float h = Input.GetAxis("Horizontal");
            if(currentState == Stand) transform.position += h * Time.deltaTime * speed * Vector3.right;
            if (opponent)
            {
                if (opponent.transform.position.x < transform.position.x) transform.localScale = new Vector3(-1, 1, 1);
                else transform.localScale = new Vector3(1, 1, 1);
            }
            if (Standing() && Input.GetButtonDown("Punch"))
            {
                SetMove(1);
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
