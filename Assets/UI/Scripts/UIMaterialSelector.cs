using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputManager;

public class UIMaterialSelector : MonoBehaviour {
	public List<Material> materials = new List<Material>();
	// Use this for initialization
	int current = 0;
	LZFightPlayer controller;
    public LZFightPlayer Controller
    {
        set
        {
            controller = value;
        }
    }
	public LZFighterAnimator fighter;
	SpriteRenderer renderer;
	bool stop = false;
	bool ready = false;
    public bool active = false;
	public bool Ready {
        set
        {
            ready = value;
        }
		get {
			return ready;
		}
	}

	void Start () {
		renderer = fighter.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
        if (!active) return;
		if (!stop && !ready){
			if (controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.LEFT).GetDown()) {
				current = (current - 1) % materials.Count;
			}
			if (controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.RIGHT).GetDown()) {
				current = (current + 1) % materials.Count;
			}
			if (current < 0) {
				current = materials.Count - 1;
			}
			Debug.Log (current);
			renderer.material = materials [current];
			stop = true;
		}
		if (stop && !(controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.LEFT).Get() || controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.RIGHT).Get())) {
			stop = false;
		}
		if (controller.GetInput(LZFight.LZFIGHTERINPUTEVENT.ATTACK).GetDown()) {
			ready = true;
		}
	}

    public Material GetCurrentMaterial()
    {
        return materials[current];
    }
}
