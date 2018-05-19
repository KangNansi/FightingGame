using UnityEngine;

public static class RectTransformExtension
{
    public static MoveTo MoveTo(this RectTransform transform, Vector3 origin, Vector3 target, float duration = 0f)
    {
        return (MoveTo)
            GlobalScriptMachine
            .Launch(new MoveTo()
            {
                transform = transform,
                origin = origin,
                target = target,
                duration = duration
            });
    }

    public static MoveTo MoveTo(this RectTransform transform, Vector3 target, float duration = 0f)
    {
        return transform.MoveTo(transform.position, target, duration);
    }
}