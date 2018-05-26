using UnityEngine;
using System.Collections;

public class FrameTimer {
    private float time = 0.0f;
    public float Time {
        get {
            return time;
        }
    }
    public bool absolute;

    public void Update() {
        time += (absolute ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime);
    }

    public void Reset() {
        time = 0;
    }

}
