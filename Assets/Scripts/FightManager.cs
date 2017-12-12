using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class FightManager : MonoBehaviour {
        public FighterController player1;
        public FighterController player2;
        public static float groundHeight = 0;
        public static float gravity = 1;
        public static float defaultTimeModifier = 1.0f;
        public static float timeModifier = 1.0f;

        bool bReset = false;
        float resetTimer = 0.0f;


	    // Use this for initialization
	    void Start () {
            player1.Reset();
            player2.Reset();
            groundHeight = player1.transform.position.y;
        }
	
	    // Update is called once per frame
	    void Update () {
            FighterController.Hit(player1, player2);
            if (!bReset && (player1.Life <= 0 || player2.Life <= 0))
            {
                Debug.Log("Match End");
                bReset = true;
                resetTimer = 0.0f;
            }
            else if (bReset)
            {
                resetTimer += Time.deltaTime;
                if(resetTimer > 2.0f)
                {
                    ResetMatch();
                }
            }
	    }

        void ResetMatch()
        {
            Debug.Log("Match Reload");
            bool bPlayer1Fall = (player1.Life <= 0);
            bool bPlayer2Fall = (player2.Life <= 0);
            player1.Reset();
            player2.Reset();
            if (bPlayer1Fall)
            {
                player1.Fighter.SetMove(player1.Fighter.GetUp);
            }

            if (bPlayer2Fall)
            {
                player2.Fighter.SetMove(player1.Fighter.GetUp);
            }
            bReset = false;
        }

    }
}
