using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {
    VirtualController controller;
    public List<Button> buttons;
    int current = 0;
    bool stop = false;
	// Use this for initialization
	void Start () {
        GameConfiguration gconf = GameConfiguration.instance;
        if(gconf != null)
        {
            controller = gconf.p1controller;
        }
	}
	
	// Update is called once per frame
	void Update () {
        buttons[current].Select();
        float v = controller.GetVertical();
        if (Mathf.Abs(v) < 0.1f)
        {
            stop = false;
        }
        else if(!stop)
        {
            if(v < 0.1f)
            {
                current--;
                if (current < 0)
                {
                    current = buttons.Count - 1;
                }
            }
            else
            {
                current++;
                if (current >= buttons.Count)
                {
                    current = 0;
                }
            }
            stop = true;
        }
        if (controller.GetJumpDown())
        {
            buttons[current].OnSubmit(null);
        }
	}

	public void Quit(){
		Application.Quit();
	}

	public void LaunchVersus(){
		SceneManager.LoadScene("CharacterChoice");
	}
}
