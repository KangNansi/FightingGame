using FightingGame;
using UnityEngine;

namespace LZFight.Scripting {
    [CreateAssetMenu(menuName = "FighterScript/TauntConfirm")]
    public class TauntConfirm : MiniScript {
        public WWiseEventScriptable tauntWeak;
        public WWiseEventScriptable tauntStrong;
        public float delay;

        public override void OnStart() {
            base.OnStart();
            float value = fighter.GetComponent<Combo>().CurrentCombo;
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
            if(timer.Time > delay)
            {
                fighter.GetComponent<Combo>().Confirm();
                return true;
            }
            
            return false;
        }

        public override void OnEnd() {
            base.OnEnd();
        }
    }
}
