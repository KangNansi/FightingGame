using UnityEngine;
using System.Collections;
using CameraScript;
using System.Collections.Generic;

public static class CameraExtension {

    public static void Shake(this Camera camera, float strength, float frequency, float time) {
        Shake shake = new Shake();
        shake.camera = camera;
        shake.strength = strength;
        shake.frequency = frequency;
        shake.time = time;
        GlobalScriptMachine.Launch(shake);
    }

    public static void LimitX(this Camera camera, float minX, float maxX) {
        GlobalScriptMachine
            .Launch(new LimitX() {
                camera = camera,
                minX = minX,
                maxX = maxX
            });
    }

    public static FollowMultiple FollowMultiple(this Camera camera, params Transform[] targets) {
        return (FollowMultiple)
            GlobalScriptMachine
                .Launch(new FollowMultiple() {
                    camera = camera,
                    targets = targets,
                });
    }
}
