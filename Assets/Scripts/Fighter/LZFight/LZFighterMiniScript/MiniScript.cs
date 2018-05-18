using UnityEngine;

namespace LZFight {
    public abstract class MiniScript : ScriptableObject {
        public float lifeTime = -1;
        protected FrameTimer timer = new FrameTimer();
        protected LZFighter fighter;

        public virtual void Initialize(LZFighter fighter) {
            this.fighter = fighter;
        }

        public virtual void OnStart() {

        }

        public virtual bool OnUpdate() {
            timer.Update();
            return true;
        }

        public virtual void OnEnd() {

        }
    }
}