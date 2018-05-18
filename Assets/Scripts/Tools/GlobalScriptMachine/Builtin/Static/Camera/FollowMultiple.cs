using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CameraScript {
    public class FollowMultiple : ScriptCamera {


        public Transform[] targets;
        public Vector3 offset = Vector3.zero;

        public override void Start() {
            base.Start();

        }

        public override bool Update() {
            base.Update();
            if(targets.Length < 0) {
                return true;
            }
            Vector3 middle = targets[0].position;
            for(int i = 1; i < targets.Length; i++) {
                middle += targets[i].position;
            }
            middle /= targets.Length;
            camera.transform.position = middle + offset;// + refPoint;

            
            return false;
        }

        public override int GetPriority() {
            return -100;
        }

    }
}