using UnityEngine;
using System.Collections;

public static class TransformExtension
{
    public static ClampToCamera ClampToCamera(this Transform transform, Camera camera, float threshold)
    {
        return (ClampToCamera)
            GlobalScriptMachine
            .Launch(new ClampToCamera(){
                transform = transform,
                camera = camera,
                threshold = threshold
            });
    }
}
