using UnityEngine;
using System.Collections;

public class FrameTimer {
    private float time = 0.0f;
    public float Time {
        get {
            return time;
        }
    }

    public void Update() {
        time += UnityEngine.Time.deltaTime;
    }

    public void Reset() {
        time = 0;
    }

}
