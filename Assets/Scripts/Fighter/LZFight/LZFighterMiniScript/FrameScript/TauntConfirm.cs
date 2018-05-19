using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/TauntConfirm")]
    public class TauntConfirm : MiniScript {
        public WWiseEventScriptable tauntWeak;
        public WWiseEventScriptable tauntStrong;

        public override void OnStart() {
            base.OnStart();
            float value = fighter.GetComponent<Combo>().Confirm();
            if(value > 30)
            {
                fighter.AddScript(tauntWeak);
            }
            else
            {
                fighter.AddScript(tauntStrong);
            }
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
