using UnityEngine;
using System.Collections;

public class MoveTo : ScriptRectTransform
{

    public Vector3 origin;
    public Vector3 target;
    public float duration;

    public override void Start()
    {
        base.Start();
        if(duration <= 0f)
        {
            transform.anchoredPosition = target;
            return;
        }
        origin = transform.anchoredPosition;
    }

    public override bool Update()
    {
        base.Update();
        transform.anchoredPosition = Vector3.Lerp(origin, target, Time() / duration);
        return Time() > duration;
    }
}
