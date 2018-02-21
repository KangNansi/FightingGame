using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICharacterSelector : MonoBehaviour {
	public UIMaterialSelector p1;
	public UIMaterialSelector p2;

	// Use this for initialization
	void Start () {
        p1.Controller = GameConfiguration.instance.p1controller;
        p2.Controller = GameConfiguration.instance.p2controller;
    }
	
	// Update is called once per frame
	void Update () {
		if (p1.Ready && p2.Ready) {
			SceneManager.LoadScene ("level1");
		}
        GameConfiguration gconf = GameConfiguration.instance;
        gconf.p1material = p1.GetCurrentMaterial();
        gconf.p2material = p2.GetCurrentMaterial();
	}
}
