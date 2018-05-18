using UnityEngine;
using System.Collections;

namespace CameraScript {
    public class LimitX : ScriptCamera {

        public float minX;
        public float maxX;

        public override bool Update() {
            base.Update();

            Transform transform = camera.transform;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);

            return false;
        }

        public override int GetPriority() {
            return -55;
        }

    }
}