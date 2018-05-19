using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LZFight {
    [Serializable]
    public class StateMachineNode {
#if UNITY_EDITOR
        public Rect nodeRect = new Rect(0, 0, 100, 100);
        public bool isDragged = false;
#endif
        public string name;
        [NonSerialized]
        LZFighter fighter;
        public LZFighterStateData data;
        [NonSerialized]
        public LZFighterStateData currentData;
        [NonSerialized]
        StateMachine machine;

        bool leaf = true;
        public int parent = -1;
        public List<int> containedNodes = new List<int>();
        public int startState = 0;

        public bool loop = false;

        public Vector2 velocity;
        public bool invert = false;

        public List<MiniScript> scripts = new List<MiniScript>();

        [SerializeField]
        bool isShortcut;
        public bool IsShortcut{
            get { 
                return isShortcut;
            }
        }
        [SerializeField]
        int targetState;
        public int Target {
            get {
                return targetState;
            }
        }

        FrameTimer time = new FrameTimer();

        public StateMachineNode(bool isShortcut = false, int target = 0) {
            if (isShortcut) {
                this.isShortcut = true;
                this.targetState = target;
            }
        }

        public void Initialize(LZFighter fighter, StateMachine machine) {
            this.fighter = fighter;
            this.machine = machine;
            time = new FrameTimer();
        }

        public void OnStart(bool jump = false) {
            if (isShortcut) {
                fighter.stateMachine.JumpToState(targetState);
                return;
            }
            fighter.AddScript(scripts);
            time.Reset();
            if (containedNodes.Count <= 0) {
                currentData = UnityEngine.Object.Instantiate(data);
                currentData.Initialize(fighter);
                currentData.Invert = invert;
                currentData.OnStart();
            }
            else {
                if (!jump) {
                    machine.PushState(startState);
                }
            }
        }

        public bool OnUpdate() {
            time.Update();
            if(containedNodes.Count <= 0) {
                return currentData.OnUpdate();
            }
            return false;
        }

        public void OnEnd() {

        }
    }
}
