using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/Block")]
    public class Block : MiniScript {
        public LZFighterStateTransition.Input input;

        public GameObject blockParticle;

        private Combo combo;

        private FrameTimer blockTimer = new FrameTimer();

        public WWiseEventScriptable block;

        private bool disabled = false;
        public bool Disabled {
            get {
                return disabled;
            }
            set {
                disabled = value;
            }
        }

        public override void OnStart() {
            base.OnStart();
            combo = fighter.GetComponent<Combo>();
            combo.onReceiveDamage += onDamaged;
        }

        public override bool OnUpdate() {
            base.OnUpdate();
            blockTimer.Update();

            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
            if(combo != null) {
                combo.onReceiveDamage -= onDamaged;
            }
        }

        private bool onDamaged(float dmg, Vector3 position) {
            if(fighter.stateMachine.allowInput && fighter.ValidateInput(input) && !disabled) {
                Debug.Log("Blocked");
                if (fighter.ApplyEvent(LZFIGHTEREVENT.BLOCK)) {
                    Debug.Log("blocking");
                }
                fighter.AddScript(block);
                GameObjectExtension.InstantiateParticleAndDestroy(blockParticle, position, fighter.invertHorizontal);
                return true;
            }
            return false;
        }
    }
}
