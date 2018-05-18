using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LZFight.Scripting;

namespace LZFight {
    public class LZFighterStateData : State {

        [SerializeField]
        public List<LZFighterFrame> frames = new List<LZFighterFrame>();

        public Vector2 velocity;

        private bool invert = false;
        public bool Invert {
            set {
                invert = value;
            }
        }


        int current = 0;
        float frameTime = 0;

        public LZFighterStateData() {
            
        }

        public override void OnStart() {
            base.OnStart();
            if (invert) {
                current = frames.Count - 1;
            }
            else {
                current = 0;
            }
            frameTime = 0f;
        }

        public override bool OnUpdate() {
            base.OnUpdate();
            bool ended = false;

            frameTime += Time.deltaTime;
            if(frameTime > frames[current].time) {
                if (invert) {
                    current--;
                }
                else {
                    current++;
                }

                if (frames.Count <= current || current < 0) {
                    ended = true;
                    fighter.ApplyEvent(LZFIGHTEREVENT.STATEEND);
                    return true;
                }
            
                if(current < 0) {
                    current += frames.Count;
                }
                current %= frames.Count;
                frameTime = 0.0f;

                fighter.stateMachine.allowInput = GetFrame().frameType == LZFighterFrame.Type.RecoveryFrame;
                ApplyHitBox();
                fighter.AddScript(GetFrame().scripts);
            }
            return ended;
        }

        public override LZFighterFrame GetFrame() {
            return frames[current];
        }

        private void ApplyHitBox() {
            LZFighterFrame frame = GetFrame();
            HitboxComponent hc = fighter.gameObject.GetComponent<HitboxComponent>();
            if(hc == null) {
                Debug.LogWarning(fighter.name + ": No hitbox component found");
                return;
            }
            hc.SetDefense(frame.hitboxes.FindAll((h) => h._type == FightingGame.HitBox.Type.Body));
            hc.SetAttack(frame.hitboxes.FindAll((h) => h._type == FightingGame.HitBox.Type.Attack));
        }
    }
}
