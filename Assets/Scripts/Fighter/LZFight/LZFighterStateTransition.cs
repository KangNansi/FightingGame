using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LZFight {
    [System.Serializable]
    public class LZFighterStateTransition {

        public int source;
        public int target;


        [SerializeField]
        public List<LZFIGHTEREVENT> events = new List<LZFIGHTEREVENT>();
        public enum INPUT_TYPE {
            VALUE, DOWN, UP
        }
        [System.Serializable]
        public class Input {
            public INPUT_TYPE type;
            public LZFIGHTERINPUTEVENT ev;
            public bool no = false;
        }
        [SerializeField]
        public List<Input> inputs = new List<Input>();

        public List<Condition> scriptedConditions = new List<Condition>();

        public List<MiniScript> scripts = new List<MiniScript>();


        public LZFighterStateTransition() {
        }

        public bool HasEvent(LZFIGHTEREVENT fighterEvent) {
            return events.Contains(fighterEvent);
        }

        public bool HasEvent(LZFIGHTERINPUTEVENT fighterEvent, INPUT_TYPE type) {
            return inputs.FindIndex((p) => p.type == type && p.ev == fighterEvent) != -1;
        }

        public bool HasEvent(Enum fighterEvent) {
            if(fighterEvent is LZFIGHTEREVENT) {
                return HasEvent((LZFIGHTEREVENT)fighterEvent);
            }
            else if (fighterEvent is LZFIGHTERINPUTEVENT) {
                return HasEvent((LZFIGHTERINPUTEVENT)fighterEvent, INPUT_TYPE.DOWN);
            }
            Debug.LogWarning("HasEvent: Invalid Enum type.");
            return false;
        }

        public bool IsEmpty() {
            return events.Count + inputs.Count + scriptedConditions.Count == 0; 
        }
    }
}
