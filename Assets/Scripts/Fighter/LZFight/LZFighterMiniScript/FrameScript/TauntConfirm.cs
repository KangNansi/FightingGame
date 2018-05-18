using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/TauntConfirm")]
    public class TauntConfirm : MiniScript {

        public override void OnStart() {
            base.OnStart();
            fighter.GetComponent<Combo>().Confirm();
        }

        public override bool OnUpdate() {
            base.OnUpdate();

            return true;
        }

        public override void OnEnd() {
            base.OnEnd();
        }
    }
}
