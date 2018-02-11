using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class FireBall : MonoBehaviour {

        HitBox hitbox = new HitBox(HitBox.Type.Attack, Vector2.zero, Vector2.zero);
        public FighterController opponent;
        public Vector3 velocity;
        public float dmg;
        public float guardDmg;
        public float stun;

	    // Use this for initialization
	    void Start () {
            hitbox._position = new Vector2(-0.25f, -0.25f);
            hitbox._size = new Vector2(0.5f, 0.5f);
            hitbox.dmg = dmg;
            hitbox.guardDmg = guardDmg;
            hitbox.stun = stun;
	    }
	
	    // Update is called once per frame
	    void Update () {
            opponent.Hit(hitbox, transform);
            transform.position += Time.deltaTime * velocity;
	    }

        
    }
}
