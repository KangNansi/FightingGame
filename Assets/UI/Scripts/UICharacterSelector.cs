using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICharacterSelector : MonoBehaviour {
	public UIMaterialSelector p1;
	public UIMaterialSelector p2;

    VirtualController c1;
    VirtualController c2;

	// Use this for initialization
	void Start () {
        c1 = GameConfiguration.instance.p1controller;
        p1.Controller = c1;
        c2 = GameConfiguration.instance.p2controller;
        p2.Controller = c2;
    }
	
	// Update is called once per frame
	void Update () {
		if (p1.Ready && p2.Ready) {
			SceneManager.LoadScene("level1");
		}
        if (Input.GetKeyDown(c1.Dash))
        {
            if (p1.Ready)
            {
                p1.Ready = false;
            }
            else
            {
                SceneManager.LoadScene("Title");
            }
        }
        if (Input.GetKeyDown(c2.Dash))
        {
            if (p2.Ready)
            {
                p2.Ready = false;
            }
            else
            {
                SceneManager.LoadScene("Title");
            }
        }
        GameConfiguration gconf = GameConfiguration.instance;
        gconf.p1material = p1.GetCurrentMaterial();
        gconf.p2material = p2.GetCurrentMaterial();
	}
}
