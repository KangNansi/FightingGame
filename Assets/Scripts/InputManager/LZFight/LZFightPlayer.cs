using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LZFight;
using UnityEngine;

namespace InputManager {
    [CreateAssetMenu()]
    public class LZFightPlayer : LZFightBase{
        [System.Serializable]
        public class EventInputPair {
            public LZFIGHTERINPUTEVENT ev;
            public InputObject input = new InputObject();
        }
        public List<EventInputPair> pairs = new List<EventInputPair>();
        protected override void Calculate(float deltaTime) {
            foreach(var kvp in pairs) {
                if (kvp.input != null && kvp.input.Get()) {
                    eventQueue.Enqueue(kvp.ev);
                    return;
                }
            }
        }

        public InputObject GetInput(LZFIGHTERINPUTEVENT ev) {
            return pairs.Find((p) => p.ev == ev).input;
        }

    }
}
