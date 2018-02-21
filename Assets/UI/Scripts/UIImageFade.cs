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
	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		image.color = new Color (image.color.r, image.color.g, image.color.b, curve.Evaluate (time/length));
	}

	public void Launch(){
		time = 0;
	}
}
