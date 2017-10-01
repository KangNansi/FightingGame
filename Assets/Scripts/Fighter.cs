using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [ExecuteInEditMode]
    public class Fighter : MonoBehaviour {
        public float speed;
        [SerializeField]
        public List<FighterState> states = new List<FighterState>();
        public int currentState = 0;

        public MoveSet moveSet;

        // Use this for initialization
        void Start () {
		
	    }

        public string[] GetStateList()
        {
            List<string> r = new List<string>();
            for(int i =0; i<states.Count; i++)
            {
                r.Add("state"+i);
            }
            return r.ToArray();
        }
	
	    // Update is called once per frame
	    void Update () {
            float h = Input.GetAxis("Horizontal");
            transform.position += h * Time.deltaTime * speed * Vector3.right;
	    }

        public void OnRenderObject()
        {
            Debug.Log("Rendering");
            states[currentState].Draw(transform.localToWorldMatrix);
        }

    }
}
