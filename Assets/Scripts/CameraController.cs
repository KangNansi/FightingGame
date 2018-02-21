using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	new public Camera camera;

    public static CameraController controller = null;

    float ShakeTimer = 100f;
	public float shakeFrequency = 100f;
	float shakeStrength = 1f;

    public AnimationCurve animationCurve;
	public float curveMultiplier;


	// Use this for initialization
	void Start () {
        controller = this;
	}
	
	// Update is called once per frame
	void Update () {
		camera.transform.localPosition = Vector2.zero
			+ new Vector2 (Mathf.PerlinNoise(ShakeTimer*shakeFrequency, -ShakeTimer*shakeFrequency), Mathf.PerlinNoise (-ShakeTimer*shakeFrequency+100, ShakeTimer*shakeFrequency+100))*animationCurve.Evaluate(ShakeTimer/curveMultiplier)*shakeStrength;
		ShakeTimer += Time.deltaTime;
	}

	public void Shake(float strength = 1.0f){
		ShakeTimer = 0;
		shakeStrength = strength;
	}
}
