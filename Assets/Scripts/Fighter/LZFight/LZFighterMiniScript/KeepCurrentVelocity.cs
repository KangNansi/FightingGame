using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/KeepCurrentVelocity")]
    public class KeepCurrentVelocity : MiniScript {
        Vector2 velocity;

        public override void OnStart() {
            base.OnStart();
            velocity = fighter.GetVelocity();
            fighter.velocityModifier += GetVelocity;
        }

        public override bool OnUpdate() {
            base.OnUpdate();
            if (lifeTime > 0 && timer.Time > lifeTime)
                return true;
            else
                return false;
        }

        public override void OnEnd() {
            base.OnEnd();
            fighter.velocityModifier -= GetVelocity;
        }

        private Vector2 GetVelocity(Vector2 current) {
            return current + velocity * (1 - (timer.Time/lifeTime));
        }
    }
}
