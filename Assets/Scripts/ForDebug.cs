using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ForDebug : MonoBehaviour {

    public bool SlowMo;

    public bool PauseOnTaunt = false;

	// Use this for initialization
	void Start () {
        ChangeTime();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.X)) {
            SlowMo = !SlowMo;
            ChangeTime();
        }
	}

    void ChangeTime() {
        if (SlowMo) {
            Time.timeScale = 0.5f;
        }
        else {
            Time.timeScale = 1f;
        }
    }
}
