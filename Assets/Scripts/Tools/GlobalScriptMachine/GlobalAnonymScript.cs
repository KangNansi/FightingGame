using UnityEngine;
using System.Collections;
using System;

public class GlobalAnonymScript : GlobalScript {

    public Action start;
    public Func<bool> update;
    public Action end;

    public override void Start() {
        base.Start();
        if(start != null) {
            start.Invoke();
        }
    }

    public override bool Update() {
        base.Update();
        if(update != null) {
            return update.Invoke();
        }
        return kill;
    }

    public override void End() {
        base.End();
    }
}
