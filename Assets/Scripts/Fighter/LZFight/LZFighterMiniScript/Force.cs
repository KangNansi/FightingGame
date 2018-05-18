using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/Force")]
    public class Force : MiniScript {
        public AnimationCurve curve;
        public Vector2 direction;
        public float strength;
        public bool cancelOnHitGround = false;
        private bool forceStop = false;

        public override void OnStart() {
            base.OnStart();
            fighter.velocityModifier += GetStrength;
            if (cancelOnHitGround) {
                fighter.onEvent += ListenEvent;
            }
            Debug.Log("Hop: "+strength+" "+lifeTime);
        }

        public override bool OnUpdate() {
            base.OnUpdate();
            if ((lifeTime > 0 && timer.Time > lifeTime) || forceStop)
                return true;
            else
                return false;
        }

        public override void OnEnd() {
            base.OnEnd();
            if (cancelOnHitGround) {
                fighter.onEvent -= ListenEvent;
            }
            Debug.Log("Ended");
            fighter.velocityModifier -= GetStrength;
        }

        private Vector2 GetStrength(Vector2 current) {
            float modifier = (lifeTime >= 0) ? curve.Evaluate(timer.Time / lifeTime) : 1f;
            return current + direction * modifier * strength;
        }

        private void ListenEvent(LZFIGHTEREVENT ev) {
            if(ev == LZFIGHTEREVENT.HITGROUND) {
                forceStop = true;
            }
        }
    }
}
