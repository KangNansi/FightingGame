using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZFight;
using UnityEngine;

namespace InputManager {
    public abstract class LZVirtualController<T> : ScriptableObject {
        protected Queue<T> eventQueue = new Queue<T>();
        public T defaultValue;

        public virtual bool GetEvent(float deltaTime, out T ev) {
            Calculate(deltaTime);
            if (eventQueue.Count > 0) {
                ev = eventQueue.Dequeue();
                return true;
            }
            else {
                ev = defaultValue;
                return false;
            }
        }

        protected abstract void Calculate(float deltaTime);
    }
}
