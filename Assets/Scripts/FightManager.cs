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
        public static int matchNumber = 2;

        int p1victory = 0;
        int p2victory = 0;
        

        bool bReset = false;
        float resetTimer = 0.0f;

        public delegate void SetupVictory(int n);
        public static event SetupVictory setupVictory;
        public delegate void AddVictory(FighterController player);
        public static event AddVictory addVictory;

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
                if(player1.Life <= 0)
                {
                    p2victory++;
                    addVictory(player2);
                }
                if(player2.Life <= 0)
                {
                    p1victory++;
                    addVictory(player1);
                }
                Debug.Log("Match End");
                bReset = true;
                resetTimer = 0.0f;
                OnMatchEnd();
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

        void OnMatchEnd()
        {
            if (p1victory >= matchNumber)
            {
                Debug.Log("Player 1 wins");
            }
            else if(p2victory >= matchNumber)
            {
                Debug.Log("Player 2 wins");
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

        void ResetFight()
        {
            p1victory = 0;
            p2victory = 0;
            ResetMatch();
        }

    }
}
