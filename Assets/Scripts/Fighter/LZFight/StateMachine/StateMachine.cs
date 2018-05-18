using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LZFight {

    [Serializable]
    public class StateContainer : IDContainer<StateMachineNode> {

    }

    [System.Serializable]
    public class StateMachine {




        public StateContainer states = new StateContainer();

        public List<LZFighterStateTransition> transitions = new List<LZFighterStateTransition>();

        public bool allowInput = true;

        public int startState = 0;

        private List<int> stateStack = new List<int>(); 

        public StateMachineNode this[int index] {
            get {
                return states[index];
            }
        }
        public StateMachineNode CurrentState {
            get {
                return states[stateStack.Last()];
            }
        }
        List<LZFighterStateTransition> CurrentTransitions {
            get {
                return transitions.FindAll((t) => t.source == stateStack.Last());
            }
        }

        LZFighter fighter;

        FrameTimer time;

        public void Initialize(LZFighter fighter) {
            this.fighter = fighter;
            foreach(var s in states) {
                s.Initialize(fighter, this);
            }
            for(int i = 0; i < states.Count; i++) {
                states[i].containedNodes = states.FindAll((s) => s.parent == i).ConvertAll((s) => states.FindIndex((a) => a == s));
            }
            time = new FrameTimer();
        }

        public void Start() {
            PushState(startState);
        }

        public bool Update() {
            time.Update();
            if (allowInput && !fighter.blockInput) {
                for (int i = stateStack.Count - 1; i >= 0; i--) {
                    if (ValidateInputDirectTransitions(stateStack[i])) {
                        i = -1;
                    }
                }
            }

            return CheckStateEnd();
        }

        private bool CheckStateEnd() {
            if (CurrentState.OnUpdate()) {
                for (int i = stateStack.Count - 1; i >= 0; i--) {
                    if (CurrentState.loop) {
                        LoopState();
                        return true;
                    }
                    if (!OnCurrentStateEnd() && i > 0) {
                        PopState();
                    }
                    else {
                        return true;
                    }
                }
                return true;
            }
            return false;
        }

        public bool OnCurrentStateEnd() {
            List<LZFighterStateTransition> currentTransitions = CurrentTransitions;
            foreach(var t in currentTransitions) {
                if (t.IsEmpty()) {
                    ApplyTransition(t);
                    return true;
                }
            }
            return false;
        }

        public bool ApplyEvent(Enum ev) {
            for (int i = stateStack.Count - 1; i >= 0; i--) {
                if (ApplyEvent(ev, stateStack[i])) {
                    return true;
                }
            }
            return false;
        }

        private bool ApplyEvent(Enum ev, int state) {
            List<LZFighterStateTransition> trans = GetStateTransitions(state);
            foreach(var t in trans) {
                if (t.HasEvent(ev)) {
                    PopToState(state);
                    ApplyTransition(t);
                    return true;
                }
            }
            return false;
        }

        private bool ValidateInputDirectTransitions(int state) {
            List<LZFighterStateTransition> currentTransition = GetStateTransitions(state);

            currentTransition.Sort((A, B) => B.inputs.Count - A.inputs.Count);
            foreach (var transition in currentTransition) {
                bool hasInput = false;
                bool result = true;

                // INPUTS
                if (transition.inputs.Count > 0) {
                    hasInput = true;
                    result &= ValidateInputs(transition.inputs);
                }

                // CONDITIONS
                if (transition.scriptedConditions.Count > 0) {
                    hasInput = true;
                    result &= ValidateCondition(transition.scriptedConditions);
                }

                bool conditions = ValidateCondition(transition.scriptedConditions);

                if (result && hasInput) {
                    PopToState(state);
                    ApplyTransition(transition);
                    return true;
                }
            }
            return false;
        }

        private bool ValidateCondition(List<Condition> scriptedConditions) {
            bool result = true;

            foreach (var condition in scriptedConditions) {
                result &= condition.Verified(fighter);
            }

            return result;
        }

        private bool ValidateInputs(List<LZFighterStateTransition.Input> inputs) {
            bool result = true;

            foreach (var input in inputs) {
                LZFIGHTERINPUTEVENT inputEvent = input.ev;
                if (fighter.invertHorizontal) {
                    if (inputEvent == LZFIGHTERINPUTEVENT.RIGHT) {
                        inputEvent = LZFIGHTERINPUTEVENT.LEFT;
                    }
                    else if (inputEvent == LZFIGHTERINPUTEVENT.LEFT) {
                        inputEvent = LZFIGHTERINPUTEVENT.RIGHT;
                    }
                }
                InputObject inputObject = fighter.controller.GetInput(inputEvent);
                bool value = false;
                switch (input.type) {
                    case LZFighterStateTransition.INPUT_TYPE.DOWN:
                        value = inputObject.GetDown();
                        break;
                    case LZFighterStateTransition.INPUT_TYPE.UP:
                        value = inputObject.GetUp();
                        break;
                    case LZFighterStateTransition.INPUT_TYPE.VALUE:
                        value = inputObject.Get();
                        break;
                    default:
                        break;
                }
                result &= (input.no)?!value:value;
            }
            return result;
        }

        public LZFighterFrame GetCurrentFrame() {
            return CurrentState.currentData.GetFrame();
        }

        private void ApplyTransition(LZFighterStateTransition transition) {
            bool jump = false;
            fighter.AddScript(transition.scripts);
            if (states[transition.source].IsShortcut) {
                JumpToState(transition.target);
                jump = true;
            }
            else {
                NextState(transition.target);
            }
            if (fighter.debug) {
                Debug.Log(fighter.frameNumber + "|" + fighter.name + ": " + states[transition.source].name + "->" + states[transition.target].name + "||" + jump);
            }
        }

        public List<LZFighterStateTransition> GetStateTransitions(int state) {
            List<LZFighterStateTransition> result = transitions.FindAll((t) => t.source == state);
            List<int> shortcuts = states.FindAllIndex((s) => s.IsShortcut && s.Target == state);
            foreach(var id in shortcuts) {
                result.AddRange(GetStateTransitions(id));
            }
            return result;
        }

        public List<LZFighterStateTransition> GetChildrenTransitions(int state) {
            if(state < 0) {
                return transitions.FindAll((t) => states[t.source].parent < 0);
            }
            else {
                return transitions.FindAll((t) => states[t.source].parent == state);
            }
        }

        public List<StateMachineNode> GetChildren(int state) {
            if(state < 0) {
                return states.FindAll((s) => s.parent < 0);
            }
            else {
                return states.FindAll((s) => s.parent == state);
            }
        }

        public string GetStatePath(int state) {
            string path = states[state].name;
            int currentState = state;
            while(states[currentState].parent >= 0) {
                currentState = states[currentState].parent;
                path = states[currentState].name + "/" + path;
            }
            return path;
        }

        public void LogPath() {
            string path = fighter.frameNumber+"[";
            foreach(var s in stateStack) {
                path += states[s].name + "/";
            }
            path.Remove(path.Length - 1);
            path += "]";
            Debug.Log(path);
        }

        public void PushState(int state, bool jump = false) {
            //Debug.Log(fighter.frameNumber+"|Pushing state:" + startState);
            stateStack.Add(state);
            states[state].OnStart(jump);
        }

        public void NextState(int state) {
            CurrentState.OnEnd();
            stateStack[stateStack.Count - 1] = state;
            states[state].OnStart();
        }

        public void JumpToState(int state) {
            CurrentState.OnEnd();
            stateStack.Clear();
            int currentState = state;
            Stack<int> path = new Stack<int>();
            path.Push(state);
            while (states[currentState].parent >= 0) {
                currentState = states[currentState].parent;
                path.Push(currentState);
            }
            while(path.Count > 0) {
                if (path.Count == 1) {
                    PushState(path.Pop());
                }
                else {
                    PushState(path.Pop(), true);
                }

            }
        }

        public void LoopState() {
            CurrentState.OnEnd();
            CurrentState.OnStart();
        }

        public void PopState() {
            CurrentState.OnEnd();
            stateStack.RemoveAt(stateStack.Count - 1);
        }

        public void PopToState(int state) {
            while(stateStack[stateStack.Count-1] != state) {
                PopState();
            }
        }

        public void RemoveState(int state) {
            foreach(var node in states[state].containedNodes) {
                RemoveState(node);
            }
            if(states[state].parent >= 0) {
                states[states[state].parent].containedNodes.Remove(state);
            }
            transitions.RemoveAll((t) => t.source == state || t.target == state);
            states.Remove(state);
        }

        public void RemoveState(StateMachineNode state) {
            int index = states.FindIndex((s) => s == state);
            RemoveState(index);
        }

        public void CleanTransitions() {
            for(int i = 0; i < transitions.Count; i++) {
                if(!states.Contains(transitions[i].source) || !states.Contains(transitions[i].target)) {
                    transitions.RemoveAt(i);
                    i--;
                }
            }
        }

    }
}
