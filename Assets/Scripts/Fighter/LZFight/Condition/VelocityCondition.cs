using UnityEngine;

namespace LZFight {
    [CreateAssetMenu(menuName = "Conditions")]
    public class VelocityCondition : Condition {
        public Vector2 direction;
        public float threshold;

        public override bool Verified(LZFighter fighter) {
            return Vector2.Dot(direction, fighter.GetVelocity().normalized) > threshold;   
        }
    }
}