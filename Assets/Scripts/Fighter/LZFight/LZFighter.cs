using FightingGame;
using InputManager;
using LZFight.Scripting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LZFight {

    public enum LZFIGHTERINPUTEVENT {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        ATTACK,
        TAUNT,
        JUMP,
        DASH,
    }

    public enum LZFIGHTEREVENT {
        NONE,
        STATEEND,
        HITGROUND,
        BLOCK,
        HIT,
        FALL,
        DIE,
    }

    [CreateAssetMenu()]
    public class LZFighter : ScriptableObject {
        public string fighterName = "";

        [NonSerialized]
        public bool debug = false;
        [NonSerialized]
        public int frameNumber = 0;

        /// <summary>
        /// GameObject
        /// </summary>
        [NonSerialized]
        public GameObject gameObject;
        public MiniScriptMachine Machine { get; private set; }

        public List<MiniScript> initScripts = new List<MiniScript>();

        public StateMachine stateMachine = new StateMachine();

        public float time = 0;

        public bool invertHorizontal = false;

        [NonSerialized]
        public LZFightPlayer controller;
        private Queue<LZFIGHTERINPUTEVENT> inputBuffer = new Queue<LZFIGHTERINPUTEVENT>();

        public event Func<Vector2, Vector2> velocityModifier;
        public float internalVelocityStrength = 1f;

        public event System.Action<LZFIGHTEREVENT> onEvent;

        public bool blockInput = false;

        public void Awake() {
            Machine = new MiniScriptMachine(this);
            AddScript(initScripts);
            stateMachine.Initialize(this);
            stateMachine.Start();
        }

        public void Initialize(LZFightPlayer controller, GameObject gameObject) {
            this.controller = controller;
            this.gameObject = gameObject;
        }

        public void AddScript(List<MiniScript> scripts) {
            foreach(var script in scripts) {
                if(script != null)
                    Machine.AddScript(script);
            }
        }

        public void AddScript(MiniScript script)
        {
            Machine.AddScript(script);
        }

        public LZFighterFrame CurrentFrame{
            get {
                return stateMachine.GetCurrentFrame();
            }
        }
        
        public StateMachineNode CurrentState { get { return stateMachine.CurrentState; } }

        public LZFighterStateData CurrentStateData {
            get {
                return stateMachine.CurrentState.data;
            }
        }
        /*
        public LZFighterStateTransition GetTransition(int transition) {
            return transitions[transition];
        }*/

        public bool Refresh(float deltaTime) {
            frameNumber++;
            Machine.Update(deltaTime);

            foreach(var input in controller.pairs) {
                if (input.input.GetDown()) {
                    inputBuffer.Enqueue(input.ev);
                    //ApplyEvent(input.ev, LZFighterStateTransition.INPUT_TYPE.DOWN);
                }
            }
            return stateMachine.Update();
        }

        

        

        

        public bool ApplyEvent(Enum fighterEvent) {
            if(onEvent != null && fighterEvent is LZFIGHTEREVENT) {
                onEvent.Invoke((LZFIGHTEREVENT)fighterEvent);
            }
            return stateMachine.ApplyEvent(fighterEvent);
        }

        public Vector2 GetRawVelocity() {
            LZFighterFrame frame = CurrentFrame;
            LZFighterStateData state = CurrentStateData;
            StateMachineNode stateNode = stateMachine.CurrentState;
            return frame.velocity + state.velocity + stateNode.velocity;
        }

        public Vector2 GetVelocity() {
            LZFighterFrame frame = CurrentFrame;
            LZFighterStateData state = CurrentStateData;
            StateMachineNode stateNode = stateMachine.CurrentState;
            Vector2 velocity = frame.velocity + state.velocity + stateNode.velocity;
            velocity *= internalVelocityStrength;
            
            if(velocityModifier != null) {
                foreach(var modifier in velocityModifier.GetInvocationList()) {
                    velocity = (Vector2)modifier.DynamicInvoke(velocity);
                }
                //velocity /= velocityModifier.GetInvocationList().Length + 1f;
            }

            if (invertHorizontal) {
                velocity = new Vector2(-velocity.x, velocity.y);
            }
            return velocity;
        }

        public T GetComponent<T>() where T : MiniScript {
            return Machine.GetComponent<T>();
        }
        
        public bool ValidateInput(LZFighterStateTransition.Input input) {
            LZFIGHTERINPUTEVENT inputEvent = input.ev;
            if (invertHorizontal) {
                if (inputEvent == LZFIGHTERINPUTEVENT.RIGHT) {
                    inputEvent = LZFIGHTERINPUTEVENT.LEFT;
                }
                else if (inputEvent == LZFIGHTERINPUTEVENT.LEFT) {
                    inputEvent = LZFIGHTERINPUTEVENT.RIGHT;
                }
            }
            InputObject inputObject = controller.GetInput(inputEvent);
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
            return value;
        }
    }
}
