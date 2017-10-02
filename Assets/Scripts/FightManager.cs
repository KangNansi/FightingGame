using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class FightManager : MonoBehaviour {
        public Fighter player1;
        public Fighter player2;
        public static float groundHeight = 0;
        public static float gravity = 1;

	    // Use this for initialization
	    void Start () {
		
	    }
	
	    // Update is called once per frame
	    void Update () {
            Fighter.Hit(player1, player2);
	    }
    }
}
