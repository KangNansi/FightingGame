using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITime : MonoBehaviour {
	Text timerText;
	// Use this for initialization
	void Start () {
		timerText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timerText != null && FightingGame.FightManager.Instance != null) {
			timerText.text = FightingGame.FightManager.Instance.MatchTime.ToString();
		}
	}
}
