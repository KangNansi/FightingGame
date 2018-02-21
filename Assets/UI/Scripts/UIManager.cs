using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame{
	public class UIManager : MonoBehaviour {

		public UIImageFade VictoryP1;
		public UIImageFade VictoryP2;
		public UIImageFade Fight;

		// Use this for initialization
		void Awake () {
			FightManager.roundEnd += Victory;
			FightManager.fightBegin += (c) => Fight.Launch(c);
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void Victory(int player){
			if (player == 0) {
				VictoryP1.Launch (null);
			} else if (player == 1) {
				VictoryP2.Launch (null);
			}
		}
	}
}
