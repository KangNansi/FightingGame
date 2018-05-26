using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/GuardBreak")]
    public class GuardBreak : MiniScript {
        public float maxGuard = 10;
        private float currentGuard;
        public float CurrentGuard {
            get {
                return currentGuard / maxGuard;
            }
        }
        public float breakDuration = 2f;
        private FrameTimer breakTimer = new FrameTimer();

        private Block blockHandler;

        public event System.Func<Vector3, bool> onReceiveDamage;

        public WWiseEventScriptable breakSound;

        public override void OnStart() {
            base.OnStart();
            blockHandler = fighter.GetComponent<Block>();
            currentGuard = 0;
        }

        public override bool OnUpdate() {
            base.OnUpdate();
            breakTimer.Update();
            if(breakTimer.Time > breakDuration) {
                blockHandler.Disabled = false;
            }
            
            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }

        public void AddGuardBreak(float guard, Vector3 position) {
            if(onReceiveDamage != null) {
                foreach(var handle in onReceiveDamage.GetInvocationList()) {
                    if ((bool)handle.DynamicInvoke(position)) {
                        return;
                    }
                }
            }

            currentGuard += guard;

            if(currentGuard > maxGuard) {
                fighter.AddScript(breakSound);
                blockHandler.Disabled = true;
                breakTimer.Reset();
                currentGuard = 0;
            }
            currentGuard = Mathf.Clamp(currentGuard, 0, maxGuard);
        }
    }
}
