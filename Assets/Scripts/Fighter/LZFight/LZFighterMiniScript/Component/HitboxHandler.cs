using FightingGame;
using System.Collections.Generic;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/HitboxHandler")]
    public class HitboxHandler : MiniScript {

        Life life;
        Combo combo;
        Stun stun;
        GuardBreak guardBreak;

        public GameObject particle;

        public List<ScriptableScript> scripts = new List<ScriptableScript>();

        public override void OnStart() {
            base.OnStart();
            life = fighter.GetComponent<Life>();
            combo = fighter.GetComponent<Combo>();
            stun = fighter.GetComponent<Stun>();
            guardBreak = fighter.GetComponent<GuardBreak>();
        }

        public override bool OnUpdate() {
            base.OnUpdate();

            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }

        public void ReceiveHit(HitBox hitbox) {
            GlobalScriptMachine.Launch(scripts);
            bool applied = false;
            if (combo != null) {
                applied = combo.ReceiveDamage(hitbox.dmg, hitbox.GetCenter() + Vector3.forward * fighter.gameObject.transform.position.z);
            }

            if (applied)
            {
                particle.InstantiateAndDestroy(hitbox.GetCenter() + Vector3.forward * fighter.gameObject.transform.position.z, 2f);
            }

            if (stun != null && applied) {
                stun.AddStun(hitbox.stun, hitbox.GetCenter() + Vector3.forward * fighter.gameObject.transform.position.z);
            }
            
            
            if(guardBreak != null && !applied) {
                guardBreak.AddGuardBreak(hitbox.guardDmg, hitbox.GetCenter() + Vector3.forward * fighter.gameObject.transform.position.z);
            }

        }
    }
}
