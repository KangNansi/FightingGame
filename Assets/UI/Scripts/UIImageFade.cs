using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageFade : MonoBehaviour {
	float time = 500;
	public float length = 2f;
	public AnimationCurve curve;
	Image image;
    bool launched = false;
    System.Action EndAction;
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		image.color = new Color (image.color.r, image.color.g, image.color.b, curve.Evaluate (time/length));
        if (launched && time > 1f)
        {
            if(EndAction != null)
            {
                EndAction.Invoke();
            }
        }
	}

	public void Launch(System.Action onEnd){
		time = 0;
        launched = true;
        EndAction = onEnd;
	}
}
