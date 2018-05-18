using UnityEngine;
using System.Collections;

using LZFight;

public class LZFighterComponent : MonoBehaviour {

    protected LZFighter fighter;
    protected event System.Action OnFighterSet;
    

    public void SetFighter(LZFighter fighter) {
        this.fighter = fighter;
        if(OnFighterSet != null) {
            OnFighterSet.Invoke();
        }
    }
}
