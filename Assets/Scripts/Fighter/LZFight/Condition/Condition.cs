using UnityEngine;

namespace LZFight {
    public abstract class Condition : ScriptableObject {
        public abstract bool Verified(LZFighter fighter);
    }
}