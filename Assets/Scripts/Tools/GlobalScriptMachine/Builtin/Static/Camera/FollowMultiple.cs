using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CameraScript {
    public class FollowMultiple : ScriptCamera {


        public Transform[] targets;
        public Vector3 offset = Vector3.zero;

        public override void Start() {
            base.Start();
            offset = camera.transform.position - GetMiddle();
        }

        public override bool Update() {
            base.Update();
            Vector3 middle = GetMiddle();
            camera.transform.position = middle + offset;// + refPoint;

            
            return false;
        }

        private Vector3 GetMiddle()
        {
            if (targets.Length < 0)
            {
                return Vector3.zero;
            }
            Vector3 middle = targets[0].position;
            for (int i = 1; i < targets.Length; i++)
            {
                middle += targets[i].position;
            }
            middle /= targets.Length;
            return middle;
        }

        public override int GetPriority() {
            return -100;
        }

    }
}