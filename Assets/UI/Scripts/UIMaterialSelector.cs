using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaterialSelector : MonoBehaviour {
	public List<Material> materials = new List<Material>();
	// Use this for initialization
	int current = 0;
	VirtualController controller;
    public VirtualController Controller
    {
        set
        {
            controller = value;
        }
    }
	public FightingGame.FighterController fighter;
	SpriteRenderer renderer;
	bool stop = false;
	bool ready = false;
	public bool Ready {
		get {
			return ready;
		}
	}

	void Start () {
		renderer = fighter.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!stop){
			if (controller.GetHorizontal () < -0.1) {
				current = (current - 1) % materials.Count;
			}
			if (controller.GetHorizontal () > 0.1) {
				current = (current + 1) % materials.Count;
			}
			if (current < 0) {
				current = materials.Count - 1;
			}
			Debug.Log (current);
			renderer.material = materials [current];
			stop = true;
		}
		if (stop && Mathf.Abs(controller.GetHorizontal()) < 0.1) {
			stop = false;
		}
		if (controller.GetPDown ()) {
			ready = true;
		}
	}

    public Material GetCurrentMaterial()
    {
        return materials[current];
    }
}
