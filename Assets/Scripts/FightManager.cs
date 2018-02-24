using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        bool running = true;

        public delegate void SetupVictory(int n);
        public static event SetupVictory setupVictory;
        public delegate void AddVictory(FighterController player);
        public static event AddVictory addVictory;

		public delegate void RoundEnd(int player);
		public static event RoundEnd roundEnd;

        public delegate void FightBegin(System.Action onEnd);
		public static event FightBegin fightBegin;

        private void Awake()
        {
            GameConfiguration config = GameConfiguration.instance;
            if(config != null)
            {
                player1.controller = config.p1controller;
                player2.controller = config.p2controller;
                player1.GetComponent<SpriteRenderer>().material = config.p1material;
                player2.GetComponent<SpriteRenderer>().material = config.p2material;
                matchTime = config.config.matchTime;
                if (config.p1isAI)
                {
                    AIController ai = player1.gameObject.AddComponent<AIController>();
                    ai.AIReflex = config.AIReflex;
                }
                if (config.p2isAI)
                {
                    AIController ai = player2.gameObject.AddComponent<AIController>();
                    ai.AIReflex = config.AIReflex;
                }
            }
        }

        // Use this for initialization
        void Start () {
			instance = this;
			player1basePosition = player1.transform.position;
			player2basePosition = player2.transform.position;
            player1.Reset();
            player2.Reset();
            groundHeight = player1.transform.position.y;
			if (fightBegin != null) {
                Block();
				fightBegin.Invoke(UnBlock);
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
                    if (!OnMatchEnd())
                    {
                        ResetMatch();
					    if (fightBegin != null) {
                            Block();
						    fightBegin.Invoke(UnBlock);
					    }
                    }
                }
            }
            if (running)
            {
			    time += Time.deltaTime;
            }
	    }

        void Block()
        {
            running = false;
            player1.running = false;
            player2.running = false;
        }

        void UnBlock()
        {
            running = true;
            player1.running = true;
            player2.running = true;
        }

        bool OnMatchEnd()
        {
            if (p1victory >= matchNumber)
            {
                SceneManager.LoadScene("CharacterChoice");
                return true;
            }
            else if(p2victory >= matchNumber)
            {
                SceneManager.LoadScene("CharacterChoice");
                return true;
            }
            return false;
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
