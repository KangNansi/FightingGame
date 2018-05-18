using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/HitboxHandler")]
    public class HitboxHandler : MiniScript {

        Life life;
        Combo combo;
        Stun stun;
        GuardBreak guardBreak;

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
            bool applied = false;
            if (combo != null) {
                applied = combo.ReceiveDamage(hitbox.dmg, hitbox.GetCenter());
            }

            if (stun != null && applied) {
                stun.AddStun(hitbox.stun, hitbox.GetCenter());
            }
            
            
            if(guardBreak != null && !applied) {
                guardBreak.AddGuardBreak(hitbox.guardDmg, hitbox.GetCenter());
            }

        }
    }
}
