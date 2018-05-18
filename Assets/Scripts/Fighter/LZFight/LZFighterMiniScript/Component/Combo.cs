using FightingGame;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/Combo")]
    public class Combo : MiniScript {
        private float currentCombo;
        public float CurrentCombo {
            get {
                return currentCombo;
            }
        }
        public float NormalizedCombo {
            get {
                return currentCombo / life.maxLife;
            }
        }
        private Life life;

        public List<ScriptableScript> onHit = new List<ScriptableScript>();

        public event Func<float, Vector3, bool> onReceiveDamage;
        public event System.Action onConfirm;

        public override void OnStart() {
            base.OnStart();
            currentCombo = 0;
            life = fighter.GetComponent<Life>();
        }

        public override bool OnUpdate() {
            base.OnUpdate();

            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }

        public bool ReceiveDamage(float dmg, Vector3 position) {
            if(onReceiveDamage != null) {
                foreach(var handle in onReceiveDamage.GetInvocationList()) {
                    if ((bool)handle.DynamicInvoke(dmg, position)) {
                        return false;
                    }
                }
            }

            GlobalScriptMachine.Launch(onHit);

            fighter.ApplyEvent(LZFIGHTEREVENT.HIT);
            currentCombo += dmg;
            currentCombo = Mathf.Clamp(currentCombo, 0, life.CurrentLife);
            Debug.Log("Damaged");
            return true;
        }

        public void Confirm() {
            if(onConfirm != null) {
                onConfirm.Invoke();
            }
        }

        public void Apply() {
            life.Damage(currentCombo, Vector3.zero);
            if(currentCombo > 0) {
                fighter.ApplyEvent(LZFIGHTEREVENT.FALL);
            }
            currentCombo = 0;
        }
    }
}
