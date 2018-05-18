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
        GameConfiguration gconf = GameConfiguration.instance;
		if (p1.Ready && (p2.Ready || gconf.p2isAI)) {
            if (gconf.p2isAI)
            {
                gconf.p2material = p2.materials[Random.Range(0, p2.materials.Count)];
            }
			SceneManager.LoadScene("level1");
            return;
		}
        p2.active = !gconf.p2isAI;
        if (Input.GetKeyDown(c2.P))
        {
            gconf.p2isAI = false;
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
            if (!gconf.p2isAI)
            {
                gconf.p2isAI = true;
            }
            else if (p2.Ready)
            {
                p2.Ready = false;
            }
            else
            {
                SceneManager.LoadScene("Title");
            }
        }
        gconf.p1material = p1.GetCurrentMaterial();
        gconf.p2material = p2.GetCurrentMaterial();
	}
}
