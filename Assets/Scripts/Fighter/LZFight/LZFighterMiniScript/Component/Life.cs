using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/Life")]
    public class Life : MiniScript {
        public float maxLife = 10;
        private float currentLife;
        public float CurrentLife {
            get {
                return currentLife;
            }
        }
        public float NormalizedLife {
            get {
                return currentLife / maxLife;
            }
        }

        public event System.Func<Vector3, bool> onReceiveDamage;

        public override void OnStart() {
            base.OnStart();
            currentLife = maxLife;
        }

        public override bool OnUpdate() {
            base.OnUpdate();

            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }

        public bool Damage(float dmg, Vector3 position) {
            if(onReceiveDamage != null) {
                foreach(var handle in onReceiveDamage.GetInvocationList()) {
                    if ((bool)handle.DynamicInvoke(position)) {
                        return false;
                    }
                }
            }
            currentLife -= dmg;
            if(currentLife <= 0) {
                fighter.ApplyEvent(LZFIGHTEREVENT.DIE);
            }
            Debug.Log("Damaged");
            return true;
        }
    }
}
