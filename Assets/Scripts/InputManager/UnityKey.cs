using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InputManager {
    public enum KeyType {
        CODE, AXIS
    };
    public class UnityKey {
        public string axis;

        public float Get() {
            return Input.GetAxis(axis);
        }

        public bool KeyDown() {
            return Input.GetKeyDown(axis);
        }

        public bool KeyUp() {
            return Input.GetKeyUp(axis);
        }
    }
}
