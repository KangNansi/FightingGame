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
				return (int)(Mathf.Clamp(matchTime-time, 0, 99));
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

		public delegate void RoundEnd(int player);
		public static event RoundEnd roundEnd;

		public static event System.Action FightBegin;

	    // Use this for initialization
	    void Start () {
			instance = this;
			player1basePosition = player1.transform.position;
			player2basePosition = player2.transform.position;
            player1.Reset();
            player2.Reset();
            groundHeight = player1.transform.position.y;
			if (FightBegin != null) {
				FightBegin.Invoke();
			}
        }
	
	    // Update is called once per frame
	    void Update () {
            FighterController.Hit(player1, player2);
			if (!bReset && (player1.Life <= 0 || player2.Life <= 0 || time > matchTime))
            {
				if(player1.Life < player2.Life)
                {
                    p2victory++;
                    addVictory(player2);
					if (roundEnd != null) {
						roundEnd.Invoke (1);
					}
                }
                if(player2.Life < player1.Life)
                {
                    p1victory++;
                    addVictory(player1);
					if (roundEnd != null) {
						roundEnd.Invoke (0);
					}
                }
                Debug.Log("Match End");
                bReset = true;
                resetTimer = 0.0f;
            }
            else if (bReset)
            {
                resetTimer += Time.deltaTime;
                if(resetTimer > 2.0f)
                {
					OnMatchEnd();
                    ResetMatch();
					if (FightBegin != null) {
						FightBegin.Invoke();
					}
                }
            }
			time += Time.deltaTime;
	    }

        void OnMatchEnd()
        {
            if (p1victory >= matchNumber)
            {
				
            }
            else if(p2victory >= matchNumber)
            {
				
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
			time = 0;
        }

        void ResetFight()
        {
            p1victory = 0;
            p2victory = 0;
            ResetMatch();
        }

    }
}
