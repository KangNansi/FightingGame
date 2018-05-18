using UnityEngine;
using UnityEditor;

namespace LZFight {
    public abstract class Condition : ScriptableObject {
        public abstract bool Verified(LZFighter fighter);
    }
}