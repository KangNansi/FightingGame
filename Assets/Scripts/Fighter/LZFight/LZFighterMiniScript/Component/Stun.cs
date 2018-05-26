using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/Stun")]
    public class Stun : MiniScript {
        public float maxStun = 10;
        private float currentStun;
        public float CurrentStun {
            get {
                return currentStun / maxStun;
            }
        }

        public event System.Func<Vector3, bool> onReceiveDamage;

        public override void OnStart() {
            base.OnStart();
            currentStun = 0;
        }

        public override bool OnUpdate() {
            base.OnUpdate();

            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }

        public void AddStun(float stun, Vector3 position) {
            if(onReceiveDamage != null) {
                foreach(var handle in onReceiveDamage.GetInvocationList()) {
                    if ((bool)handle.DynamicInvoke(position)) {
                        return;
                    }
                }
            }
            
            currentStun += stun;

            if(currentStun > maxStun) {
                if (fighter.ApplyEvent(LZFIGHTEREVENT.FALL)) {
                    currentStun = 0;
                }
            }
            currentStun = Mathf.Clamp(currentStun, 0, maxStun);
        }
    }
}
