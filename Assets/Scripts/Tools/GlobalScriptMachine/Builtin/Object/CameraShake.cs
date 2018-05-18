using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableScript/Camera/Shake")]
public class CameraShake : ScriptableScript {

    public float strength;
    public AnimationCurve strengthCurve;
    public float frequency;
    public float time;
    

    public override void Start() {
        base.Start();
        Camera.main.Shake(strength * strengthCurve.Evaluate(Time()/time), frequency, time);
    }

    public override bool Update() {
        Debug.Log("Update");
        return true;
    }
}
