using UnityEngine;
using System.Collections;

public class ClampToCamera : ScriptTransform
{
    public Camera camera;

    public float threshold;

    public override bool Update()
    {
        base.Update();

        Vector3 positionInCam = camera.WorldToViewportPoint(transform.position);
        positionInCam.Set(Mathf.Clamp(positionInCam.x, threshold, 1-threshold), Mathf.Clamp(positionInCam.y, threshold, 1-threshold), positionInCam.z);
        transform.position = camera.ViewportToWorldPoint(positionInCam);

        return false;
    }
}
