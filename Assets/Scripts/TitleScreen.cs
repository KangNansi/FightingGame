using InputManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {
    LZFightPlayer controller;
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

            if(controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.UP).GetDown())
            {
                current--;
                if (current < 0)
                {
                    current = buttons.Count - 1;
                }
            }
            else if (controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.DOWN).GetDown())
            {
                current++;
                if (current >= buttons.Count)
                {
                    current = 0;
                }
            }

        if (controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.ATTACK).Get())
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
