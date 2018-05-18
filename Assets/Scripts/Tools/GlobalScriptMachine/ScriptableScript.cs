using UnityEngine;
using System.Collections;

public class ScriptableScript : ScriptableObject, IScript {
    FrameTimer timer = new FrameTimer();
    protected bool kill;
    public int priority;

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

    public IScript Get() {
        return Instantiate(this);
    }

    public float Time() {
        return timer.Time;
    }

    public int GetPriority() {
        return priority;
    }
}
