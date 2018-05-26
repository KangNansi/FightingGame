using UnityEngine;
using System.Collections;

public class TimeBend : GlobalScript
{
    public float time;
    public AnimationCurve curve;

    public override bool Update()
    {
        base.Update();

        float value = curve.Evaluate(Time() / time);
        UnityEngine.Time.timeScale = value;
        return Time() > time;
    }

    public override void End()
    {
        base.End();
        UnityEngine.Time.timeScale = 1;
    }
}
