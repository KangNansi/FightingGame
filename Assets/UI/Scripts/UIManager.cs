using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame{
	public class UIManager : MonoBehaviour {

		public Image VictoryP1;
		public Image VictoryP2;

		// Use this for initialization
		void Start () {
			FightManager.roundEnd += Victory;
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		
		void Victory(int player){
			if (player == 0) {
				StartCoroutine(VictoryImage(VictoryP1));
			} else if (player == 1) {
				StartCoroutine(VictoryImage(VictoryP2));
			}
		}
		
		IEnumerator VictoryImage(Image victoryImage){
			for (int i = 0; i < 40; i++) {
				victoryImage.color = new Color(1,1,1,i / 40.0f);
				yield return new WaitForSeconds(0.005f);
			}
			yield return new WaitForSeconds (1f);
			victoryImage.color = new Color(1,1,1,0);
		}
	}
}
