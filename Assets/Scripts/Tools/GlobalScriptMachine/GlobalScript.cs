using UnityEngine;
using System.Collections;
using System;

public class GlobalScript : IScript {
    FrameTimer timer = new FrameTimer()
    {
        absolute = true
    };
    protected bool kill;

    public virtual void Start() {

    }

    public virtual bool Update() {
        timer.Update();
        return kill;
    }

    public virtual void End() {

    }
    
    public void Kill() {
        kill = true;
    }

    public float Time() {
        return timer.Time;
    }

    public virtual int GetPriority() {
        return 0;
    }
}
