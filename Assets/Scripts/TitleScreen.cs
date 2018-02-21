using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Quit(){
		Application.Quit();
	}

	public void LaunchVersus(){
		SceneManager.LoadScene("CharacterChoice");
	}
}
