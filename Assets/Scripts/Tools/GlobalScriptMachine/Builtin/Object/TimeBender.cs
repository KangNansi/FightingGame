using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableScript/Camera/TimeBending")]
public class TimeBender : ScriptableScript
{
    public float time;
    public AnimationCurve curve;

    public override void Start()
    {
        base.Start();
        GlobalScriptMachine.Launch(new TimeBend(){
            time = time,
            curve = curve
        });
        Kill();
    }
}
