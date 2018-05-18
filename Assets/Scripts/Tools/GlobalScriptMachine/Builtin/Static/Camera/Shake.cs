using UnityEngine;
using System.Collections;

namespace CameraScript {
    public class Shake : ScriptCamera {

        public float strength;
        public AnimationCurve strengthCurve;
        public float frequency;
        public float time;

        private Vector3 movement = Vector3.zero;

        public override void Start() {
            base.Start();
        }

        public override bool Update() {
            base.Update();
            float seedX = strength * (Mathf.PerlinNoise(Time() * frequency, -777 + Time() * frequency)-0.6f);
            float seedY = strength * (Mathf.PerlinNoise(Time() * frequency, 25 + Time() * frequency) - 0.6f);
            float seedZ = 0;


            Vector3 relMove = new Vector3(seedX, seedY, seedZ);
            movement += relMove;
            camera.transform.position += relMove;
            if (Time() > time) {
                return true;
            }

            return false;
        }

        public override void End() {
            base.End();
            Camera.main.transform.position -= movement;
        }
    }
}