using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICharacterSelector : MonoBehaviour {
	public UIMaterialSelector p1;
	public UIMaterialSelector p2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (p1.Ready && p2.Ready) {
			SceneManager.LoadScene ("level1");
		}
	}
}
