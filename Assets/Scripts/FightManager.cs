using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame
{
    public class FightManager : MonoBehaviour {

		static FightManager instance = null;
		public static FightManager Instance {
			get{
				return instance;
			}
		}
			

        public FighterController player1;
		Vector3 player1basePosition;
        public FighterController player2;
		Vector3 player2basePosition;
        public static float groundHeight = 0;
        public static float gravity = 1f;
        public static float defaultTimeModifier = 1.0f;
        public static float timeModifier = 1.0f;
        public static int matchNumber = 2;

		float time = 0.0f;
		public float MatchTime {
			get {
				return time;
			}
		}
		public float matchTime = 99;

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
			instance = this;
			player1basePosition = player1.transform.position;
			player2basePosition = player2.transform.position;
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
			time += Time.deltaTime;
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
			player1.transform.position = player1basePosition;
			player2.transform.position = player2basePosition;
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
